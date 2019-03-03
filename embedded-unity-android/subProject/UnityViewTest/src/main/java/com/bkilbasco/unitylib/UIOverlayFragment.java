package com.bkilbasco.unitylib;

import android.app.Fragment;
import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;

import com.unity3d.player.UnityPlayer;


/**
 * The UIOverlayFragment class adds a simple UI that
 *  does essentially the same thing as the back
 *  button - go back to the previous activity. This
 *  will dismiss the UnityPlayerActivity, shutting down
 *  the unity player.
 *
 */
public class UIOverlayFragment extends Fragment {


    /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
    ///
    ///     onCreateView
    ///------------------------------------------
    /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {

        return inflater.inflate( R.layout.fragment_uioverlay, container, false );

    }//END onCreateView



    /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
    ///
    ///     onViewCreated
    ///------------------------------------------
    /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
    @Override
    public void onViewCreated(View view, Bundle savedInstanceState) {

        super.onViewCreated(view, savedInstanceState);

        // Back button
        //
        Button  btnBack = (Button)view.findViewById(R.id.btnBack);
        btnBack.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                getActivity().finish();

            }//END onClick
        });//END


        // Zoom out button
        //
        Button  btnZoomOut = (Button)view.findViewById(R.id.btnZoomOut);
        btnZoomOut.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                // Send zoom out message
                UnityPlayer.UnitySendMessage( "NativeBridge", "ZoomOut", "2" );

            }//END onClick
        });//END


        // Zoom in button
        //
        Button  btnZoomIn = (Button)view.findViewById(R.id.btnZoomIn);
        btnZoomIn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                // Send zoom in message
                UnityPlayer.UnitySendMessage( "NativeBridge", "ZoomIn", "2" );

            }//END onClick
        });//END


    }//END onViewCreated


}//END class
