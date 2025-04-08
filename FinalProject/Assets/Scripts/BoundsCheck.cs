//using System.Numerics;
using JetBrains.Annotations;
using UnityEngine;

public class BoundsCheck : MonoBehaviour
{

    [System.Flags]
    public enum eScreenLocs {
        onScreen = 0, 
        offRight = 1, 
        offLeft = 2, 
        offUp = 4, 
        offDown = 8 
    }

    public enum eType {center, inset, outset};

    [Header ("Inscribed")]
    public eType boundsType = eType.center;
    public float radius = 1f;
    public bool keepOnScreen = true;

    [Header("Dynamic")]
    public eScreenLocs screenLocs = eScreenLocs.onScreen;
    public float camWidth;
    public float camHeight;
    public float TopScreen;
    //public bool isOnScreen = true;

    void Awake()
    {
        camHeight = Camera.main.orthographicSize;
        TopScreen = -.7f;
        camWidth = camHeight * Camera.main.aspect;
    }

    void LateUpdate()
    {

        float checkRadius = 0;
        if (boundsType == eType.inset) checkRadius = -radius;
        if (boundsType == eType.outset) checkRadius = radius;
        
        Vector3 pos = transform.position;
        screenLocs = eScreenLocs.onScreen;
        //isOnScreen = false;

        if (pos.x > camWidth + checkRadius){
            pos.x = camWidth + checkRadius;
            screenLocs |= eScreenLocs.offRight;
            //isOnScreen = false;
        }

        if (pos.x < -camWidth + checkRadius){
            pos.x = -camWidth + checkRadius;
            screenLocs |= eScreenLocs.offLeft;
            //isOnScreen = false;
        }

        if (pos.y > TopScreen + checkRadius){
            pos.y = TopScreen + checkRadius;
            screenLocs |= eScreenLocs.offUp;
            //isOnScreen = false;
        }

        if (pos.y < -camHeight + checkRadius){
            pos.y = -camHeight + checkRadius;
            screenLocs |= eScreenLocs.offDown;
           // isOnScreen = false;
        }

        if (keepOnScreen && !isOnScreen){
            transform.position = pos;
           // isOnScreen = true;
        }


    }

    public bool isOnScreen {
        get { return ( screenLocs == eScreenLocs.onScreen ); }
    }

    public bool LocIs( eScreenLocs checkLoc ) {
        if ( checkLoc == eScreenLocs.onScreen ) return isOnScreen;
        return ( (screenLocs & checkLoc) == checkLoc );
    }

}
