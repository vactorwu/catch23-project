// PBNISubclass.cpp : PBNI class
#include "PBNISubclass.h"


// default constructor
PBNISubclass::PBNISubclass()
{
}

// member variables
IPB_Session * m_pSession;
pbobject m_pbobj;


PBNISubclass::PBNISubclass( IPB_Session * pSession, pbobject obj )
{
	m_pSession = pSession ;
	m_pbobj = obj;
}

// destructor
PBNISubclass::~PBNISubclass()
{
}

// method called by PowerBuilder to invoke PBNI class methods
PBXRESULT PBNISubclass::Invoke
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
		case mid_Attach:
			pbxr = this->Attach( ci, obj );
         break;
		case mid_Detach:
			pbxr = this->Detach( ci, obj );
         break;
		
		// TODO: add handlers for other callable methods

		default:
			pbxr = PBX_E_INVOKE_METHOD_AMBIGUOUS;
	}

	return pbxr;
}


void PBNISubclass::Destroy() 
{
	delete this;
}

// Method callable from PowerBuilder
PBXRESULT PBNISubclass::Attach( PBCallInfo * ci, pbobject obj )
{
	PBXRESULT	pbxr = PBX_OK;

	HWND lhwnd = (HWND) ci->pArgs->GetAt(0)->GetLong();

	if(!IsWindow(lhwnd)) return PBX_OK;

	long indexCtr = (LONG)GetProp(lhwnd, (LPCTSTR)"indexCtr");

	if(NULL == indexCtr)
	{
		indexCtr = 1;
	}
	else
	{
		indexCtr ++;
	}

	//store pbobject
	SetProp(lhwnd, GetPBClassPropID(indexCtr), (HANDLE)obj);

	if (indexCtr == 1)
	{
		WNDPROC lproc = (WNDPROC) SetWindowLong(lhwnd, 
			GWL_WNDPROC, (LONG)SubClassProc);

		SetProp(lhwnd, (LPCTSTR)"wndproc", lproc);
	}
	
	SetProp(lhwnd, (LPCTSTR)"indexCtr", (HANDLE)indexCtr);

	return pbxr;
}

PBXRESULT PBNISubclass::Detach( PBCallInfo * ci, pbobject obj )
{
	PBXRESULT	pbxr = PBX_OK;
	HWND lhwnd = (HWND) ci->pArgs->GetAt(0)->GetLong();

	long indexCtr = (LONG)GetProp(lhwnd, (LPCTSTR)"indexCtr");

	if(NULL == indexCtr)
	{
		return PBX_OK;
	}
	else
	{
		int newIndex = 0;
		//remove props and set new props
		for(int i = 1; i <= indexCtr; i ++)
		{
			pbobject currobj = (pbobject)GetProp(lhwnd, GetPBClassPropID(i));
			RemoveProp(lhwnd, GetPBClassPropID(i));
			if(currobj == obj) continue;
			newIndex ++;
			SetProp(lhwnd, GetPBClassPropID(newIndex), currobj);
		}
		indexCtr --;
	}

	if (indexCtr == 0)
	{
		WNDPROC lproc = (WNDPROC)GetProp(lhwnd, (LPCTSTR)"wndproc");

		SetWindowLong(lhwnd, GWL_WNDPROC, (LONG)lproc); 
		//remove props
		RemoveProp(lhwnd, (LPCTSTR)"indexCtr");
		RemoveProp(lhwnd, (LPCTSTR)"wndproc");
	}
	else
	{
		//update counter prop
		SetProp(lhwnd, (LPCTSTR)"indexCtr", (HWND)indexCtr);
	}

	return pbxr;
}

LPCTSTR GetPBClassPropID(long index)
{
	LPCTSTR ls;

	switch(index)
	{
		case 1: ls = (LPCTSTR)"pbobject1";break;
		case 2: ls = (LPCTSTR)"pbobject2";break;
		case 3: ls = (LPCTSTR)"pbobject3";break;
		case 4: ls = (LPCTSTR)"pbobject4";break;
		case 5: ls = (LPCTSTR)"pbobject5";break;
		case 6: ls = (LPCTSTR)"pbobject6";break;
		case 7: ls = (LPCTSTR)"pbobject7";break;
		case 8: ls = (LPCTSTR)"pbobject8";break;
		case 9: ls = (LPCTSTR)"pbobject9";break;
		case 10: ls = (LPCTSTR)"pbobject10";break;
		case 11: ls = (LPCTSTR)"pbobject11";break;
		case 12: ls = (LPCTSTR)"pbobject12";break;
		case 13: ls = (LPCTSTR)"pbobject13";break;
		case 14: ls = (LPCTSTR)"pbobject14";break;
		case 15: ls = (LPCTSTR)"pbobject15";break;
		case 16: ls = (LPCTSTR)"pbobject16";break;
		case 17: ls = (LPCTSTR)"pbobject17";break;
		case 18: ls = (LPCTSTR)"pbobject18";break;
		case 19: ls = (LPCTSTR)"pbobject19";break;
		case 20: ls = (LPCTSTR)"pbobject20";break;
	}

	return ls;
}

LRESULT APIENTRY SubClassProc(
    HWND hwnd, 
    UINT uMsg, 
    WPARAM wParam, 
    LPARAM lParam)
{

	long indexCtr = (LONG)GetProp(hwnd, (LPCTSTR)"indexCtr");
	bool processed = false;
	bool postProcess = false;

	pblong retProc = 0;
	pblong ret;
	pblong lphwnd = (pblong)hwnd;
	pblong lpmsg = (pblong)uMsg;
	pbulong lpwparam = (pbulong)wParam;
	pbulong lplparam = (pbulong)lParam;
	PBCallInfo ci;
	pbmethodID mid;
	pbclass clz;
	pbobject lpbobj;

	WNDPROC lproc = (WNDPROC)GetProp(hwnd, (LPCTSTR)"wndproc");

	for(int i = 1; i <= indexCtr; i++)
	{
		//reset post process flag
		postProcess = false;

		mid = NULL;
		clz = NULL;
		lpbobj = NULL;

		lpbobj = (pbobject)GetProp(hwnd, GetPBClassPropID(i));

		if(NULL == lpbobj) continue;

		//query for post process
		try
		{
			clz = m_pSession->GetClass(lpbobj);

			if(NULL == clz) continue;

			mid = m_pSession->GetMethodID(clz, _T("of_postprocessquery"), PBRT_FUNCTION, _T(""));
		}
		catch(...)
		{
			break;
		}

		if (m_pSession->InitCallInfo(clz, mid, &ci) == PBX_OK)
		{
			ci.pArgs->GetAt(0)->SetLong(lpmsg);

			if (m_pSession->InvokeObjectFunction(lpbobj, mid, &ci) == PBX_OK)
			{
				ret = ci.returnValue->GetLong();
				m_pSession->ProcessPBMessage();
				m_pSession->FreeCallInfo(&ci);

				postProcess = (ret != 0);
			}
		}
		
		//do post process
		if(postProcess && !processed)
		{
			CallWindowProc(lproc, hwnd, uMsg, wParam, lParam);
		}

		mid = m_pSession->GetMethodID(clz, _T("of_wndproc"), PBRT_FUNCTION, _T(""));

		if (m_pSession->InitCallInfo(clz, mid, &ci) == PBX_OK)
		{

			pbint cnt = ci.pArgs->GetCount();

			ci.pArgs->GetAt(0)->SetLong(lphwnd);
			ci.pArgs->GetAt(1)->SetLong(lpmsg);
			ci.pArgs->GetAt(2)->SetUlong(lpwparam);
			ci.pArgs->GetAt(3)->SetUlong(lplparam);

			if (m_pSession->InvokeObjectFunction(lpbobj, mid, &ci) == PBX_OK)
			{
				ret = ci.returnValue->GetLong();
				m_pSession->ProcessPBMessage();
				m_pSession->FreeCallInfo(&ci);

				if(ret != 0 && !processed)
				{
					retProc = ret;
					processed = true;
				}
			}
		}
	}

	if(processed)
	{
		if(uMsg == WM_DESTROY)
		{
			//return old proc
			SetWindowLong(hwnd, GWL_WNDPROC, (LONG)lproc); 
		}
		else
		{
			return (LONG)retProc;
		}
	}

	if (!postProcess)
	{
		return CallWindowProc(lproc, hwnd, uMsg, wParam, lParam);
	}
	else
	{
		return 0;
	}
}