// PBNIHook.h : header file for PBNI class
#ifndef PBNIHook_H
#define PBNIHook_H

#include <pbext.h>

class PBNIHook : public IPBX_NonVisualObject
{
public:
	// construction/destruction
	PBNIHook();
	PBNIHook( IPB_Session * pSession, pbobject pbobj);
	virtual ~PBNIHook();

	// IPBX_UserObject methods
	PBXRESULT Invoke
	(
		IPB_Session * session,
		pbobject obj,
		pbmethodID mid,
		PBCallInfo * ci
	);

   void Destroy();


	// PowerBuilder method wrappers
	enum Function_Entrys
	{
		mid_Register = 0,
		mid_AttachWndProc = 1,
		mid_AttachMouseProc = 2,
		mid_AttachKeyboardProc = 3,
		mid_AttachMessageProc = 4,
		mid_WndProc = 5,
		mid_MouseProc = 6,
		mid_KeyboardProc = 7,
		mid_MessageProc = 8,
		mid_Detach = 9,
		// TODO: add enum entries for each callable method
		NO_MORE_METHODS
	};

 	// methods callable from PowerBuilder
	PBXRESULT Register( PBCallInfo * ci );
	PBXRESULT AttachWndProc( PBCallInfo * ci );
	PBXRESULT AttachMouseProc( PBCallInfo * ci );
	PBXRESULT AttachKeyboardProc( PBCallInfo * ci );
	PBXRESULT AttachMessageProc( PBCallInfo * ci );
	PBXRESULT Detach( PBCallInfo * ci );
 };

LRESULT CALLBACK HookWndProc(
	int nCode,
	WPARAM wParam,
    LPARAM lParam);
LRESULT CALLBACK HookMouseProc(
	int nCode,
	WPARAM wParam,
    LPARAM lParam);
LRESULT CALLBACK HookKeyboardProc(
	int nCode,
	WPARAM wParam,
    LPARAM lParam);
LRESULT CALLBACK HookMessageProc(
	int nCode,
	WPARAM wParam,
    LPARAM lParam);
#endif	// !defined(PBNIHook_H)
