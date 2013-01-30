// PBNIHook.cpp : PBNI class
#include "PBNIHook.h"

// default constructor
PBNIHook::PBNIHook()
{
}

// member variables
IPB_Session * mh_pSession;
pbobject mh_pbobj;
HWND ihwnd;
DWORD hInstance;
LONG threadID;
//wndproc
HHOOK hhookwndproc;
int flagWndProc;
//mouseproc
HHOOK hhookmouseproc;
int flagMouseProc;
//mouseproc
HHOOK hhookkeyboardproc;
int flagKeyboardProc;
HHOOK hhookmessageproc;
int flagMessageProc;
IPB_Value **	 m_hwnd;

PBNIHook::PBNIHook( IPB_Session * pSession, pbobject pbobj)
{
	(mh_pSession = pSession, mh_pbobj = pbobj);
}

// destructor
PBNIHook::~PBNIHook()
{
}

 
// method called by PowerBuilder to invoke PBNI class methods
PBXRESULT PBNIHook::Invoke
(
	IPB_Session * session,
	pbobject obj,
	pbmethodID mid,
	PBCallInfo * ci
)
{
   PBXRESULT pbxr = PBX_OK;

	switch ( mid )
	{
		case mid_Register:
			pbxr = this->Register( ci );
         break;
		case mid_AttachWndProc:
			pbxr = this->AttachWndProc( ci );
         break;
		case mid_AttachMouseProc:
			pbxr = this->AttachMouseProc( ci );
         break;
		case mid_AttachKeyboardProc:
			pbxr = this->AttachKeyboardProc( ci );
         break;
		case mid_AttachMessageProc:
			pbxr = this->AttachMessageProc( ci );
         break;
		case mid_Detach:
			pbxr = this->Detach( ci );
         break;
		
		// TODO: add handlers for other callable methods

		default:
			pbxr = PBX_E_INVOKE_METHOD_AMBIGUOUS;
	}

	return pbxr;
}


void PBNIHook::Destroy() 
{
   delete this;
}

//wndproc
LRESULT CALLBACK HookWndProc(
	int nCode,
	WPARAM wParam,
    LPARAM lParam)
{
	if (nCode < 0)  // do not process message 
        return CallNextHookEx(hhookwndproc, nCode, 
                wParam, lParam); 

	CWPSTRUCT * lstr = (CWPSTRUCT *)lParam;
	
	if (nCode == HC_ACTION)
	{
		pbclass clz = mh_pSession->GetClass(mh_pbobj);
		pbmethodID mid = mh_pSession->GetMethodID(clz, _T("of_wndproc"), PBRT_FUNCTION, _T(""));

		PBCallInfo ci;
		if (mh_pSession->InitCallInfo(clz, mid, &ci) == PBX_OK)
		{
			pblong lphwnd = (pblong)lstr->hwnd;
			pbulong lpwparam = (pbulong)lstr->wParam;
			pbulong lplparam = (pbulong)lstr->lParam;
			pblong lpmsg = (pblong)lstr->message;

			pbint cnt = ci.pArgs->GetCount();

			ci.pArgs->GetAt(0)->SetLong(lphwnd);
			ci.pArgs->GetAt(1)->SetUlong(lpmsg);
			ci.pArgs->GetAt(2)->SetUlong(lpwparam);
			ci.pArgs->GetAt(3)->SetUlong(lplparam);

			//if (mh_pSession->TriggerEvent(mh_pbobj, mid, &ci) == PBX_OK)
			if (mh_pSession->InvokeObjectFunction(mh_pbobj, mid, &ci) == PBX_OK)
			{
				mh_pSession->ProcessPBMessage();
				pbint ret = ci.returnValue->GetInt();
				mh_pSession->FreeCallInfo(&ci);

				//if(ret != 0) return 0;
			}
		}
	}

	return CallNextHookEx(hhookwndproc, nCode, 
            wParam, lParam); 
}

//mouseproc
LRESULT CALLBACK HookMouseProc(
	int nCode,
	WPARAM wParam,
    LPARAM lParam)
{
	if (nCode < 0)  // do not process message 
        return CallNextHookEx(hhookmouseproc, nCode, 
                wParam, lParam); 

	MOUSEHOOKSTRUCT * lstr = (MOUSEHOOKSTRUCT *)lParam;
	
	if (nCode == HC_ACTION)
	{
		pbclass clz = mh_pSession->GetClass(mh_pbobj);
		pbmethodID mid = mh_pSession->GetMethodID(clz, _T("of_mouseproc"), PBRT_FUNCTION, _T(""));

		PBCallInfo ci;
		if (mh_pSession->InitCallInfo(clz, mid, &ci) == PBX_OK)
		{
			pblong lphwnd = (pblong)lstr->hwnd;
			pblong lpx = (pblong)lstr->pt.x;
			pblong lpy = (pblong)lstr->pt.y;
			pblong lpmsg = (pblong)wParam;
			pbulong lpHitTest = (pbulong)lstr->wHitTestCode;

			pbint cnt = ci.pArgs->GetCount();

			ci.pArgs->GetAt(0)->SetLong(lphwnd);
			ci.pArgs->GetAt(1)->SetLong(lpx);
			ci.pArgs->GetAt(2)->SetLong(lpy);
			ci.pArgs->GetAt(3)->SetLong(lpmsg);
			ci.pArgs->GetAt(4)->SetUlong(lpHitTest);

			//if (mh_pSession->TriggerEvent(mh_pbobj, mid, &ci) == PBX_OK)
			if (mh_pSession->InvokeObjectFunction(mh_pbobj, mid, &ci) == PBX_OK)
			{
				mh_pSession->ProcessPBMessage();
				pbint ret = ci.returnValue->GetInt();
				mh_pSession->FreeCallInfo(&ci);
			}
		}
	}

	return CallNextHookEx(hhookmouseproc, nCode, 
            wParam, lParam); 
}

//keyboardproc
LRESULT CALLBACK HookKeyboardProc(
	int nCode,
	WPARAM wParam,
    LPARAM lParam)
{
	if (nCode < 0)  // do not process message 
        return CallNextHookEx(hhookkeyboardproc, nCode, 
                wParam, lParam); 

	if (nCode == HC_ACTION)
	{
		pbclass clz = mh_pSession->GetClass(mh_pbobj);
		pbmethodID mid = mh_pSession->GetMethodID(clz, _T("of_keyboardproc"), PBRT_FUNCTION, _T(""));

		PBCallInfo ci;
		if (mh_pSession->InitCallInfo(clz, mid, &ci) == PBX_OK)
		{
			pblong lpvcode = (pblong)wParam;
			pbulong lpflags = (pbulong)lParam;

			pbint cnt = ci.pArgs->GetCount();

			ci.pArgs->GetAt(0)->SetLong(lpvcode);
			ci.pArgs->GetAt(1)->SetUlong(lpflags);

			if (mh_pSession->InvokeObjectFunction(mh_pbobj, mid, &ci) == PBX_OK)
			{
				mh_pSession->ProcessPBMessage();
				pbint ret = ci.returnValue->GetInt();
				mh_pSession->FreeCallInfo(&ci);

				//if(ret != 0) return 0;
			}
		}
	}

	return CallNextHookEx(hhookkeyboardproc, nCode, 
            wParam, lParam); 
}

//wndproc
LRESULT CALLBACK HookMessageProc(
	int nCode,
	WPARAM wParam,
    LPARAM lParam)
{
	if (nCode < 0)  // do not process message 
        return CallNextHookEx(hhookmessageproc, nCode, 
                wParam, lParam); 

	MSG * lstr = (MSG *)lParam;
	
	pbclass clz = mh_pSession->GetClass(mh_pbobj);
	pbmethodID mid = mh_pSession->GetMethodID(clz, _T("of_msgfilterproc"), PBRT_FUNCTION, _T(""));

	PBCallInfo ci;
	if (mh_pSession->InitCallInfo(clz, mid, &ci) == PBX_OK)
	{
		pblong lpcode = (pblong)nCode;
		pblong lphwnd = (pblong)lstr->hwnd;
		pbulong lpwparam = (pbulong)lstr->wParam;
		pbulong lplparam = (pbulong)lstr->lParam;
		pblong lpmsg = (pblong)lstr->message;
		pblong lptime = (pblong)lstr->time;
		pbint lpx = (pbint)lstr->pt.x;
		pbint lpy = (pbint)lstr->pt.y;

		pbint cnt = ci.pArgs->GetCount();

		ci.pArgs->GetAt(0)->SetLong(lpcode);
		ci.pArgs->GetAt(1)->SetLong(lphwnd);
		ci.pArgs->GetAt(2)->SetUlong(lpmsg);
		ci.pArgs->GetAt(3)->SetUlong(lpwparam);
		ci.pArgs->GetAt(4)->SetUlong(lplparam);
		ci.pArgs->GetAt(5)->SetLong(lptime);
		ci.pArgs->GetAt(6)->SetInt(lpx);
		ci.pArgs->GetAt(7)->SetInt(lpy);

		//if (mh_pSession->TriggerEvent(mh_pbobj, mid, &ci) == PBX_OK)
		if (mh_pSession->InvokeObjectFunction(mh_pbobj, mid, &ci) == PBX_OK)
		{
			mh_pSession->ProcessPBMessage();
			pbint ret = ci.returnValue->GetInt();
			mh_pSession->FreeCallInfo(&ci);

			//if(ret != 0) return 0;
		}
	}

	return CallNextHookEx(hhookmessageproc, nCode, 
            wParam, lParam); 
}

PBXRESULT PBNIHook::Register( PBCallInfo * ci )
{
	PBXRESULT	pbxr = PBX_OK;

	//force detach
	this->Detach(ci);

	ihwnd = (HWND) ci->pArgs->GetAt(0)->GetLong();
	hInstance = GetWindowLong ( ihwnd, GWL_HINSTANCE ) ;
	threadID = GetCurrentThreadId();

	return pbxr;
}

PBXRESULT PBNIHook::AttachWndProc( PBCallInfo * ci )
{
	PBXRESULT	pbxr = PBX_OK;

	hhookwndproc = SetWindowsHookEx( 
            WH_CALLWNDPROC, 
            HookWndProc, 
            (HINSTANCE)hInstance, threadID); 

	flagWndProc = 1;

	return pbxr;
}

PBXRESULT PBNIHook::AttachMouseProc( PBCallInfo * ci )
{
	PBXRESULT	pbxr = PBX_OK;

	hhookmouseproc = SetWindowsHookEx( 
            WH_MOUSE, 
			HookMouseProc, 
            (HINSTANCE)hInstance, threadID); 

	flagMouseProc = 1;

	return pbxr;
}

PBXRESULT PBNIHook::AttachKeyboardProc( PBCallInfo * ci )
{
	PBXRESULT	pbxr = PBX_OK;

	hhookkeyboardproc = SetWindowsHookEx( 
            WH_KEYBOARD, 
            HookKeyboardProc, 
            (HINSTANCE)hInstance, threadID); 

	flagKeyboardProc = 1;

	return pbxr;
}

PBXRESULT PBNIHook::AttachMessageProc( PBCallInfo * ci )
{
	PBXRESULT	pbxr = PBX_OK;

	hhookmessageproc = SetWindowsHookEx( 
            WH_MSGFILTER, 
            HookMessageProc, 
            (HINSTANCE)hInstance, threadID); 

	flagMessageProc = 1;

	return pbxr;
}

PBXRESULT PBNIHook::Detach( PBCallInfo * ci )
{
	PBXRESULT	pbxr = PBX_OK;

	if (flagWndProc == 1)
	{
		UnhookWindowsHookEx(hhookwndproc);
		flagWndProc = 0;
	}
	if (flagMouseProc == 1)
	{
		UnhookWindowsHookEx(hhookmouseproc);
		flagMouseProc = 0;
	}
	if (flagKeyboardProc == 1)
	{
		UnhookWindowsHookEx(hhookkeyboardproc);
		flagKeyboardProc = 0;
	}
	if (flagMessageProc == 1)
	{
		UnhookWindowsHookEx(hhookmessageproc);
		flagMessageProc = 0;
	}

	return pbxr;
}