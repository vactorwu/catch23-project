// PBNITimer.h : header file for PBNI class
#ifndef PBNITimer_H
#define PBNITimer_H

#include <pbext.h>
class PBNITimer : public IPBX_NonVisualObject
{
    IPB_Session * m_pSession;
	pbobject m_pbobject;

public:
	// construction/destruction
	PBNITimer();
	PBNITimer( IPB_Session * pSession, pbobject obj )
		:
		m_pSession(pSession),
		m_pbobject(obj)
	{
	}
	
	~PBNITimer()
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
   }


	// PowerBuilder method wrappers
	enum Function_Entrys
	{
		mid_StartTimer = 0,
		mid_StopTimer = 1,
		mid_Pulse = 2,
		// TODO: add enum entries for each callable method
		NO_MORE_METHODS
	};

	static VOID CALLBACK MyTimerProc( 
		HWND hwnd,        // handle to window for timer messages 
		UINT uMsg,     // WM_TIMER message 
		UINT idTimer,     // timer identifier 
		DWORD dwTime);

protected:
 	// methods callable from PowerBuilder
	PBXRESULT StartTimer( PBCallInfo * ci, pbobject obj);
	PBXRESULT StopTimer( PBCallInfo * ci, pbobject obj );

 };

#endif	// !defined(PBNITimer_H)