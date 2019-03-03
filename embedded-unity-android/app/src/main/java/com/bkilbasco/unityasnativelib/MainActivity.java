package com.bkilbasco.unityasnativelib;


import com.bkilbasco.unitylib.UnityPlayerActivity;
import android.app.Activity;
import android.content.Intent;
import android.graphics.Color;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;



/**
 * The MainActivity class is our root screen, and merely
 *  has a button that opens an activity with a unity
 *  player embedded in it.
 *
 */
public class MainActivity extends Activity {


    /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
    ///
    ///     onCreate
    ///------------------------------------------
    /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
    @Override
    protected void onCreate(Bundle savedInstanceState) {

        super.onCreate( savedInstanceState );
        setContentView( R.layout.activity_main );

        getWindow().getDecorView().setBackgroundColor( Color.WHITE );

        // Init open-unity button
        Button btnOpenUnity = (Button)findViewById( R.id.btnOpenUnityView );
        btnOpenUnity.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                openUnityView();

            }//END onClick
        });//END setOnClickListener


    }//END onCreate



    /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
    ///
    ///     onWindowFocusChanged
    ///------------------------------------------
    /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
    @Override
    public void onWindowFocusChanged(boolean hasFocus) {

        super.onWindowFocusChanged( hasFocus );

    }//END onWindowFocusChanged



    /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
    ///
    ///     openUnityView
    ///------------------------------------------
    /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
    protected void openUnityView(){

        Intent intent = new Intent( getApplicationContext(), UnityPlayerActivity.class );
        startActivity( intent );

    }//END openUnityView


}//END class MainActivity
