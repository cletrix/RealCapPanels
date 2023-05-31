using UnityEngine;
using System.Runtime.InteropServices;
using System;
public class TransparentWindow : MonoBehaviour
{
    [DllImport("user32.dll")]
    public static extern IntPtr GetActiveWindow();
    private struct MARGINS {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cxTopHeight;
        public int cxBottomHeight;
     
    }

    [DllImport("Dwmapi.dll")]
    private static extern uint DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS margins);

    // Start is called before the first frame update
    void Start()
    {
        IntPtr hwnd = GetActiveWindow();
        MARGINS margins = new MARGINS { cxLeftWidth = -1 };

        DwmExtendFrameIntoClientArea(hwnd, ref margins);
    }
}
