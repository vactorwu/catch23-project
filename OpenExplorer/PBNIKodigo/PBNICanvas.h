// PBNICanvas.h : header file for PBNI class
#ifndef PBNICANVAS_H
#define PBNICANVAS_H

#include <pbext.h>

class PBNICanvas : public IPBX_VisualObject
{
    // member variables
    IPB_Session * m_pSession;
	pbobject m_pbobject;
 	static HINSTANCE m_Handle;
	HWND		d_hwnd;
public:
	// construction/destruction
	PBNICanvas();
	PBNICanvas( IPB_Session * pSession, pbobject obj )
		:
		m_pSession(pSession),
		m_pbobject(obj),
		d_hwnd(NULL)

	{
	}
	
	~PBNICanvas()
	{
	}

	// IPBX_UserObject methods
	PBXRESULT Invoke
	(
		IPB_Session * session,
		pbobject obj,
		pbmethodID mid,
		PBCallInfo * ci
	);

   void Destroy()
   {
	   delete this;
	   DestroyWindow(d_hwnd);
   }

   // IPBX_VisualObject methods
   LPCTSTR GetWindowClassName();

   HWND CreateControl
	(
		DWORD dwExStyle,      // extended window style
		LPCTSTR lpWindowName, // window name
		DWORD dwStyle,        // window style
		int x,                // horizontal position of window
		int y,                // vertical position of window
		int nWidth,           // window width
		int nHeight,          // window height
		HWND hWndParent,      // handle to parent or owner window
		HINSTANCE hInstance   // handle to application instance
	);

   static void RegisterClass(); //Register the Window class
   static void UnregisterClass();//UnRegister the Window class
   static LRESULT CALLBACK WindowProc(HWND hwnd,UINT uMsg,WPARAM wParam,LPARAM lParam);//Callback Window Procedure
   static void SetDLLHandle(HANDLE hndl);//Set the dll HINSTANCE
   void TriggerEvent(HDC hdc);

	// PowerBuilder method wrappers
	enum Function_Entrys
	{
		mid_Hello = 0,
		// TODO: add enum entries for each callable method
		NO_MORE_METHODS
	};
 };

#endif	// !defined(PBNICANVAS_H)