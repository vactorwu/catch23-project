// PBNICanvas.cpp : PBNI class
#include "PBNICanvas.h"
#include <windows.h>
#include <richedit.h>
#include <commctrl.h>
#include <math.h>

HINSTANCE PBNICanvas::m_Handle = 0;

// default constructor
//PBNICanvas::PBNICanvas()
//{
//}
//
//IPB_Session * _pSession;
//pbobject _pbobj;
//
//PBNICanvas::PBNICanvas( IPB_Session * pSession, pbobject obj )
//{
//	_pSession = pSession ;
//	_pbobj = obj;
//}

// destructor
//PBNICanvas::~PBNICanvas()
//{
//}

// method called by PowerBuilder to invoke PBNI class methods
PBXRESULT PBNICanvas::Invoke
(
	IPB_Session * session,
	pbobject obj,
	pbmethodID mid,
	PBCallInfo * ci
)
{
   PBXRESULT pbxr = PBX_OK;

	//switch ( mid )
	//{
	//	case mid_Hello:
	//		pbxr = this->Hello( ci );
 //        break;
	//	
	//	// TODO: add handlers for other callable methods

	//	default:
	//		pbxr = PBX_E_INVOKE_METHOD_AMBIGUOUS;
	//}

	return pbxr;
}

// IPBX_VisualObject method
LPCTSTR PBNICanvas::GetWindowClassName()
{
   return LPCTSTR( "PBNICanvas" );
}


void PBNICanvas::SetDLLHandle(HANDLE hndl)
{
    m_Handle = (HINSTANCE)hndl;	
}

void PBNICanvas::RegisterClass()
{
   WNDCLASS wndclass;

   wndclass.style = CS_GLOBALCLASS | CS_DBLCLKS;
   wndclass.lpfnWndProc = WindowProc;
   wndclass.cbClsExtra = 0;
   wndclass.cbWndExtra = 0;
   wndclass.hInstance = m_Handle;
   wndclass.hIcon = NULL;
   wndclass.hCursor = LoadCursor (NULL, IDC_ARROW);
   wndclass.hbrBackground =(HBRUSH) (COLOR_WINDOW + 1);
   wndclass.lpszMenuName = NULL;
   wndclass.lpszClassName = _T("PBNICanvas");

   ::RegisterClass (&wndclass);
}


void PBNICanvas::UnregisterClass()
{
	::UnregisterClass(_T("PBNICanvas"),m_Handle );
}

void PBNICanvas::TriggerEvent(HDC hdc)
{
	pbclass clz = m_pSession->GetClass(m_pbobject);
	pbmethodID mid = m_pSession->GetMethodID(clz, _T("onpaint"), PBRT_EVENT, _T(""));
	pbulong lpb = (pbulong)hdc;

	PBCallInfo ci;
	m_pSession->InitCallInfo(clz, mid, &ci);

	pbint cnt = ci.pArgs->GetCount();
	//
	ci.pArgs->GetAt(0)->SetUlong(lpb);

	m_pSession->TriggerEvent(m_pbobject, mid, &ci);
	m_pSession->FreeCallInfo(&ci);

	//ci.returnValue->SetInt(0);
}

LRESULT CALLBACK PBNICanvas::WindowProc(
                                   HWND hwnd,
                                   UINT uMsg,
                                   WPARAM wParam,
                                   LPARAM lParam
                                   )
{
     
	PBNICanvas* ext = (PBNICanvas*)::GetWindowLong(hwnd, GWL_USERDATA);

	switch(uMsg)
	{
		case WM_CREATE:
			return 0;

		case WM_SIZE:
			return 0;

		case WM_COMMAND:
			return 0;

		case WM_ERASEBKGND:
			return 1;

		case WM_PAINT:
			{
				PAINTSTRUCT ps;
				HDC hdc = BeginPaint(hwnd, &ps);

				RECT rc;
				GetClientRect(hwnd, &rc);

				HDC lmemdc = CreateCompatibleDC(hdc);
				HBITMAP hmembmp = CreateCompatibleBitmap(hdc, rc.right - rc.left, rc.bottom - rc.top);
				HBITMAP oldbmp = (HBITMAP)SelectObject(lmemdc, hmembmp);

				//ext->TriggerEvent((LPCTSTR)"onpaint");
				//memdc = lmemdc;
				ext->TriggerEvent(lmemdc);
				//TriggerEvent(memdc);

				BitBlt(hdc, rc.left, rc.top, rc.right - rc.left, rc.bottom - rc.top, lmemdc, 0, 0, SRCCOPY);
				
				SelectObject(lmemdc, oldbmp);
				DeleteObject(hmembmp);
				DeleteDC(lmemdc);
				EndPaint(hwnd, &ps);
				return 0;
			}
	}

	return DefWindowProc(hwnd, uMsg, wParam, lParam);
}


// IPBX_VisualObject method
HWND PBNICanvas::CreateControl
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
)
{
   // call ::CreateWindow, ::CreateWindowEx or the appropriate creation method supplied by a control class
   // e.g.:
   d_hwnd = CreateWindowEx(
      dwExStyle,
      _T("PBNICanvas"),
      lpWindowName,
      dwStyle,
		x,
      y,
      nWidth,
      nHeight,
      hWndParent,
      NULL,
      hInstance,
      NULL
   );   
   
   ::SetWindowLong(d_hwnd, GWL_USERDATA, (LONG)this);

   return d_hwnd;
}