package com.bkilbasco.unitylib;

import com.unity3d.player.*;
import android.app.Activity;
import android.app.FragmentManager;
import android.content.Intent;
import android.content.res.Configuration;
import android.content.res.Resources;
import android.graphics.PixelFormat;
import android.os.Bundle;
import android.util.TypedValue;
import android.view.KeyEvent;
import android.view.MotionEvent;


/**
 * The UnityPlayerActivity class ...
 *
 *
 */
public class UnityPlayerActivity extends Activity {



    // The name of this object is important; we can't change
    //  its name since it is referenced from native code.
    protected UnityPlayer mUnityPlayer;



    /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
    ///
    ///     onCreate
    ///------------------------------------------
    /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
    @Override
    protected void onCreate(Bundle savedInstanceState) {

        //requestWindowFeature( Window.FEATURE_NO_TITLE );
        super.onCreate( savedInstanceState );

        getWindow().setFormat( PixelFormat.RGBX_8888 ); // <--- This makes xperia play happy

        mUnityPlayer = new UnityPlayer( this );

        setContentView( mUnityPlayer );
        mUnityPlayer.requestFocus();

        // Open UI fragment
        FragmentManager     fManager = getFragmentManager();
        UIOverlayFragment   fragment = new UIOverlayFragment();
        fManager.beginTransaction().add( android.R.id.content, fragment, "UNITY_UI_OVERLAY_TAG" ).commit();

    }//END onCreate



    /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
    ///
    ///     getPXFromDP
    ///------------------------------------------
    /** Summary: Calculates and returns a pixel value
     *  from the given density independent pixel value;
     *  converts DP to PX.*/
    /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
    private int getPXFromDP(int pDP){

        Resources r = getApplicationContext().getResources();
        return (int)TypedValue.applyDimension( TypedValue.COMPLEX_UNIT_DIP, pDP, r.getDisplayMetrics() );

    }//END getPXFromDP



    /**
     * The remaining are lifecycle methods and other event
     *  callbacks that are forwarded on to the unity player
     *
     */


    @Override protected void onNewIntent(Intent intent) {

        // To support deep linking, we need to make sure that the client can get access to
        // the last sent intent. The clients access this through a JNI api that allows them
        // to get the intent set on launch. To update that after launch we have to manually
        // replace the intent with the one caught here.
        setIntent( intent );

    }//END onNewIntent

    // Quit Unity
    @Override protected void onDestroy () {

        mUnityPlayer.destroy();
        super.onDestroy();

    }//END onDestroy

    // Pause Unity
    @Override protected void onPause() {

        super.onPause();
        mUnityPlayer.pause();

    }//END onPause

    // Resume Unity
    @Override protected void onResume() {

        super.onResume();
        mUnityPlayer.resume();

    }//END onResume

    @Override protected void onStart() {

        super.onStart();
        mUnityPlayer.start();

    }//END onStart

    @Override protected void onStop() {

        super.onStop();
        mUnityPlayer.stop();

    }//END onStop

    // Low Memory Unity
    @Override public void onLowMemory() {

        super.onLowMemory();
        mUnityPlayer.lowMemory();

    }//END onLowMemory

    // Trim Memory Unity
    @Override public void onTrimMemory(int level) {

        super.onTrimMemory( level );
        if ( level == TRIM_MEMORY_RUNNING_CRITICAL ) {
            mUnityPlayer.lowMemory();
        }//END if

    }//END onTrimMemory

    // This ensures the layout will be correct.
    @Override public void onConfigurationChanged(Configuration newConfig) {

        super.onConfigurationChanged( newConfig );
        mUnityPlayer.configurationChanged( newConfig );

    }//END onConfigurationChanged

    // Notify Unity of the focus change.
    @Override public void onWindowFocusChanged(boolean hasFocus) {

        super.onWindowFocusChanged( hasFocus );
        mUnityPlayer.windowFocusChanged( hasFocus );

    }//END onWindowFocusChanged

    // For some reason the multiple keyevent type is not supported by the ndk.
    // Force event injection by overriding dispatchKeyEvent().
    @Override public boolean dispatchKeyEvent(KeyEvent event) {

        if (event.getAction() == KeyEvent.ACTION_MULTIPLE)
            return mUnityPlayer.injectEvent( event );
        else
            return super.dispatchKeyEvent( event );

    }//END dispatchKeyEvent


    // Pass any events not handled by (unfocused) views straight to UnityPlayer
    @Override public boolean onKeyUp(int keyCode, KeyEvent event)     { return mUnityPlayer.injectEvent(event); }
    @Override public boolean onKeyDown(int keyCode, KeyEvent event)   { return mUnityPlayer.injectEvent(event); }
    @Override public boolean onTouchEvent(MotionEvent event)          { return mUnityPlayer.injectEvent(event); }
    /*API12*/ public boolean onGenericMotionEvent(MotionEvent event)  { return mUnityPlayer.injectEvent(event); }


}//END class UnityPlayerActivity
