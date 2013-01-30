// PBNITimer.cpp : PBNI class
#include "PBNITimer.h"

static IPB_Session * gSessions[100];
static pbobject gpbobjects[100];
static int timerCount;
static UINT gtimerIDs[100];
// method called by PowerBuilder to invoke PBNI class methods
PBXRESULT PBNITimer::Invoke
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
		case mid_StartTimer:
			pbxr = this->StartTimer( ci, obj );
         break;
		case mid_StopTimer:
			pbxr = this->StopTimer( ci, obj );
         break;
		
		// TODO: add handlers for other callable methods

		default:
			pbxr = PBX_E_INVOKE_METHOD_AMBIGUOUS;
	}

	return pbxr;
}


// Method callable from PowerBuilder
PBXRESULT PBNITimer::StartTimer( PBCallInfo * ci, pbobject obj )
{
	PBXRESULT	pbxr = PBX_OK;

	UINT elapse = (UINT) ci->pArgs->GetAt(0)->GetLong();

	UINT_PTR id = SetTimer(NULL, 
						NULL, 
						elapse, 
						(TIMERPROC)MyTimerProc);

	pblong ret;
	if(NULL != id)
	{
		ret = (pblong)id;

		timerCount++;

		gSessions[timerCount] = m_pSession;
		gpbobjects[timerCount] = obj;
		gtimerIDs[timerCount] = id;
	}
	else
	{
		ret = -1;
	}

	ci->returnValue->SetLong(ret);

	return pbxr;
}

PBXRESULT PBNITimer::StopTimer( PBCallInfo * ci, pbobject obj )
{
	PBXRESULT	pbxr = PBX_OK;

	UINT id = (UINT) ci->pArgs->GetAt(0)->GetLong();

	if(NULL != id)
	{
		KillTimer(NULL, id);

		timerCount--;
	}

	return pbxr;
}

VOID CALLBACK PBNITimer::MyTimerProc( 
	HWND hwnd,
	UINT uMsg,
	UINT idTimer,
	DWORD dwTime)
{
	for(int i = 1; i <= timerCount; i++)
	{
		if(idTimer == gtimerIDs[i])
		{
			pbclass clz = gSessions[i]->GetClass(gpbobjects[i]);
			pbmethodID mid = gSessions[i]->GetMethodID(clz, _T("of_pulse"), PBRT_FUNCTION, _T(""));
			PBCallInfo ci;

			if (gSessions[i]->InitCallInfo(clz, mid, &ci) == PBX_OK)
			{
				if (gSessions[i]->InvokeObjectFunction(gpbobjects[i], mid, &ci) == PBX_OK)
				{
					gSessions[i]->ProcessPBMessage();
					gSessions[i]->FreeCallInfo(&ci);
				}
			}

			return;
		}
	}
}