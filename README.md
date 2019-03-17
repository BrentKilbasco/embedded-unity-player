
[1.1]: http://i.imgur.com/tXSoThF.png (twitter icon with padding)
[2.1]: http://i.imgur.com/P3YfQoD.png (facebook icon with padding)
[3.1]: http://i.imgur.com/0o48UoR.png (github icon with padding)


[1.2]: http://i.imgur.com/wWzX9uB.png (twitter icon without padding)
[2.2]: http://i.imgur.com/fep1WsG.png (facebook icon without padding)
[3.2]: http://i.imgur.com/9I6NRUm.png (github icon without padding)


[1]: http://www.twitter.com/BrentKilbasco
[2]: http://www.facebook.com/bkilbasco
[3]: http://www.github.com/BrentKilbasco



# Native Mobile Embedded Unity Player
---
👋 Heyo! Thanks for the visit 😁

The goal of this project is to embed a Unity player window within a native Android Activity so it can be treated like any other Activity, and also to allow for sending data to the Unity player. If that sounds interesting to you, then you're at the right spot!


## 🧠 Motivation

Unity is a wonderful engine, I started using it the week the editor was released for Windows and I still absolutely love it. When running on mobile devices though, there tends to be a decent amount of overhead and Unity apps tend to be more resource intensive to run than a native app - a completely empty Unity project built to mobile will use considerably more resources than an empty Android project. Resources can sometimes end up being wasted, in a sense, especially so with apps that are largely menu based, and only display 3d content, or games, for a portion of the time the user interacts with the app. Another thing to consider is that building UI that looks and feels like native UI tends to be much more time consuming to build in Unity than in the respective native IDE.

So I was curious if there was a way, within a native mobile app, to trigger Unity to open inside of a native view so it could be treated like any other view - draw UI elements over top of it, control content within it, and create/destroy it at any given time. Turns out there’s a way! After a bit of digging around I managed to get things running. Woot!

Future implementations include full two way communication between native Android and Unity, as well as an iOS version that embeds a Unity player inside of a native UIView. 

## 📷 Screenshots

![Alt text](screenshots/ScreenShot_1.png?raw=true "Title")
![Alt text](screenshots/ScreenShot_3.png?raw=true "Title")


## 🤔 Limitations and caveats

There are a few caveats with this implementation however. The biggest being the file size. See what I did there?  🙄  ahh yea lame, ok moving on... So the file size ends up being a bit of a concern though, since there are a lot of libraries that Unity depends on. This goes for building any Unity mobile app though, but a way to help with this is to only include libraries in your C# code that you absolutely need, then set options for code stripping in the Player Settings before you export your Unity project as a Gradle project. There are more, but that's kinda out of the scope of this project. At least for now.

Another thing to be aware of is that when sending data to the Unity player object, it uses the method UnitySendMessage(), which is a bit of an expensive call. It definitely shouldn't be called every frame, or on a continual basis with a small interval. Using it in an event-based way is the preferred method. 


## 🚀 Installation - getting it up and running

First off we'll need [Android Studio](https://developer.android.com/studio). If you intend on updating the Unity project or creating your own, you'll need to install [Unity](https://unity3d.com/get-unity/update), and make sure the Android module is included in the installation process. The version of Unity used for this project is 2018.3.6f1. 

**⚠️ Important ⚠️** --> If you just want to build/run the project, you'll only need to clone the repo and then open the embedded-unity-player project in Android Studio, installing any dependencies and artifacts it recommends, and then run/build the project.

#### Clone
```
# Clone the repo
git clone https://github.com/BrentKilbasco/embedded-unity-player
```

If you want to recreate this from scratch yourself though, there are a few steps included, so let's get to it! 😃


#### 1. Set up your Unity project

* **a.** Make your fancy Unity scene
* **b.** Add that scene to the editor build settings
* **c.** Set a valid package name in Player Settings - anything with a valid format and that isn't the default package name will be fine


#### 2. Export the Unity project as a Gradle project
Now we'll need to export our Unity project as a Gradle project, checking off development build if we want to have the extra debugging options. 


#### 3. Import the newly created Gradle project as a library
Alright, so now we'll need to create a new empty Android project, then add a folder to the root folder called 'subProject', and then copy the exported Gradle project that Unity gave us into that subProject folder.

In the next steps we'll configure several gradle files and a manifest, bringing it all together. 

#### 4. Update the Android Manifest for the Unity sub project

Let's tackle that manifest file first. This file was automatically generated by Unity when we exported our project as a Gradle project, but we want to remove most of the contents. Ok, so in the AndroidManifest file in our Unity sub project, let's change it to look like:

```xml
<?xml version="1.0" encoding="utf-8"?>
<manifest
    xmlns:android="http://schemas.android.com/apk/res/android"
    xmlns:tools="http://schemas.android.com/tools"
    package="com.whatever.yourprojectnamewas" 
    android:versionName="1.0"
    android:versionCode="1"
    android:installLocation="preferExternal">
</manifest>
```


#### 5. Configure the app build.gradle file

In the app module build.gradle file, set the unity sub project as a dependency, replacing 'UnityViewTest' with whatever your exported gradle project was called. There will be other dependencies you'll need too, but that will be project specific. The part that includes the project as a dependency is:

```gradle
dependencies {
    ...
    implementation project(':subProject:UnityViewTest')
    ...
}
```

#### 6. Configure the project build.gradle file:

We'll also need to make sure we are referencing both jcenter and google in our repositories section in both the buildscript and allprojects. Both instances of repositories should look like:

```gradle
repositories {
    jcenter()
    google()
}
```

#### 7. Configure the app settings.gradle file

Next we'll need to add an extra bit of code to the settings.gradle file. You'll also need to replace 'UnityViewTest' with whatever your exported gradle project was called here as well.

```gradle
include ':app', ':subProject:UnityViewTest'
```


#### 8. Configure the sub project build.gradle file

Alright, we're almost there! A few things are involved with this step though, so let's have at it. 

So this file will be auto-generated by Unity in the gradle project export process, but there are a few important changes that need to be made for it to work nicely as a library instead of a full application. First we'll make sure jcenter and google are referenced in this gradle file, the same as we did back in step #6:

```gradle
repositories {
    jcenter()
    google()
}
```

And here again, we'll make sure they are referenced in both buildscript and appprojects sections.

Next we need to update the line
```gradle
apply plugin: 'com.android.application'
```

to

```gradle
apply plugin: 'com.android.library'
```

Now we need to get rid of the applicationId declaration and definiton since this Unity project is a library and not an app. So within the defaultConfig section, let's remove the line:

```gradle
applicationId 'com.whatever.yourpackagenamewas'
```

Finally, we'll remove the whole bundle section and everything in it. This is what my build.gradle file ended up looking like:
```gradle
buildscript {
	repositories {
		google()
		jcenter()
	}

	dependencies {
		classpath 'com.android.tools.build:gradle:3.3.1'
	}
}

allprojects {
    repositories {
        google()
        jcenter()
        flatDir {
            dirs 'libs'
        }
    }
}

apply plugin: 'com.android.library'

dependencies {
	implementation fileTree(dir: 'libs', include: ['*.jar'])
	implementation 'com.android.support.constraint:constraint-layout:1.0.2'
}

android {
	compileSdkVersion 28
	buildToolsVersion '28.0.3'

	compileOptions {
		sourceCompatibility JavaVersion.VERSION_1_8
		targetCompatibility JavaVersion.VERSION_1_8
	}

	defaultConfig {
		minSdkVersion 19
		targetSdkVersion 28
		ndk {
			abiFilters 'armeabi-v7a', 'x86'
		}
		versionCode 1
		versionName '1.0'
	}

	lintOptions {
		abortOnError false
	}

	aaptOptions {
		noCompress '.unity3d', '.ress', '.resource', '.obb'
	}

	buildTypes {
  		debug {
 			minifyEnabled false
 			useProguard false
 			proguardFiles getDefaultProguardFile('proguard-android.txt'), 'proguard-unity.txt'
  			jniDebuggable true
  		}
  		release {
 			minifyEnabled false
 			useProguard false
  			proguardFiles getDefaultProguardFile('proguard-android.txt'), 'proguard-unity.txt'
  			
  		}
	}

	packagingOptions {
		doNotStrip '*/armeabi-v7a/*.so'
		doNotStrip '*/x86/*.so'
	}

}

```

#### 9. Update the main project AndroidManifest

We'll need to make a small update to the main project's AndroidManifest to add a reference to the Activity class.

```xml
<activity android:name="com.whatever.yourlibrarynamewas.UnityPlayerActivity" />
```

#### 10. Start the UnityPlayerActivity anywhere you want

So now with that done, in our main Android app project we are able to create, destroy, start, and stop a UnityPlayerActivity whenever we want, and from whatever Activity or Fragment we want. Cool! 

Within the project I created, I just set up a super simple layout with a button anchored to the corner, and in the MainActivity.java I added the following method which I wired up to the button.

```java
protected void openUnityView(){

    Intent intent = new Intent( getApplicationContext(), UnityPlayerActivity.class );
    startActivity( intent );

}//END openUnityView
```

#### 11. Optional settings

This step is optional, but I think it's worth mentioning. Just three things I want to point out. First is that since we're embedding Unity with the native view, we probably don't want to hide the title/status bar at the top when Unity starts. We can prevent the status bar from hiding by removing the following from the UnityPlayerActivity file that Unity automatically generates in the Gradle project export process:
```java
requestWindowFeature( Window.FEATURE_NO_TITLE );
```

One other thing I'd like to mention is that adding the following line of code should make this whole set up play a little nicer on Xperia mobile devices. It can also reduce banding and help in situations where there are a lot of animated graphics. It may or may not help in your set up however, but I wanted to mention it at any rate. Okie, so the line is:

```java
getWindow().setFormat( PixelFormat.RGBX_8888 );
```

And last, but definitely most exciting, is that we're able to extend from the UnityPlayerActivity to add extra functionality, or edit the original one further if you're so inclined. In my project I just added a Fragment class with one button and a transparent background and edited the original UnityPlayerActivity to open that Fragment over top. In a larger application it would probably be a better idea to extend from it. Anyways, mine ended up looking like:

```java
@Override
protected void onCreate(Bundle savedInstanceState) {

    super.onCreate( savedInstanceState );

    getWindow().setFormat( PixelFormat.RGBX_8888 ); 

    mUnityPlayer = new UnityPlayer( this );

    setContentView( mUnityPlayer );
    mUnityPlayer.requestFocus();

    // Open UI fragment
    FragmentManager     fManager = getFragmentManager();
    UIOverlayFragment   fragment = new UIOverlayFragment();
    fManager.beginTransaction().add( android.R.id.content, fragment, "UNITY_UI_OVERLAY_TAG" ).commit();

}//END onCreate
```


#### 11. Clean and build

And that's it, awesome! 👍 Ok cool, we should now be good to clean, build, and run the project. 

## 🛠️ Updating an existing Sub-Project
Need to update an existing sub project library? Okie, let's do that!

First off, it's probably a good idea to make some back up copies of the project before updating it, or if your project is on git to commit everything before continuing so you have a fallback. This process is unfortunately rather manual. 

Alright, so let's say you make some changes to the original Unity project and export an updated Gradle project from Unity. With all the little tweaks we did to the manifest, gradle files, and UnityPlayerActivity, we can't just copy and replace the full Unity project folder within the subProject folder. A lot of the auto generated files are boilerplate anyway, and are the same as the files we already have from last Gradle project export. If your version of Unity hasn't changed since the last time you exported the project as a Gradle project, there will be only one folder in the exported project structure you'll need to worry about - the src/main/assets folder. This assets folder contains all the compiled code and assets for your Unity project. Just copy and replace the old assets folder with this new one and you're good to go. After a clean and rebuild in Android Studio of course!

If your version of Unity has changed since the last time you exported from Unity as a Gradle project, then there are a few other things you'll need to do. In this case, you'll want to copy over and replace libs/unity-classes.jar, as well as the src/main/jniLibs folder. You'll also want to check the auto generated build.gradle file, and update your existing SDK and build tools versions within the sub project build.gradle to match the newly exported Gradle project.


## 🏁 Conclusion

Phew! Ok so we made it to the end, yay! Hopefully this all made sense and things are running on your end. Of course if you come across any issues, feel free to give me a shout.


## 🤜🤛 Credits

Thanks to Penny de Byl and her videos on coding voxel worlds, which helped in creating a fun 3d object to display in the Unity player window.


## 🤷 Further Help?

Need further help? No worries! Just [get in touch with me directly](http://portfolio.bkilbasco.com) 😄

Or here:   [![alt text][1.2]][1] [![alt text][2.2]][2]  [![alt text][3.2]][3]

---

_If there are any other ideas about, or related to, this project, that you think worth mentioning - feel free to give me a shout_ 😄  

#### _Cheers!!_ 🍻 


