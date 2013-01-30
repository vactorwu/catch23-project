// PBNISubclass.h : header file for PBNI class
#ifndef PBNISubclass_H
#define PBNISubclass_H

#include <pbext.h>
class PBNISubclass : public IPBX_NonVisualObject
{
public:
	// construction/destruction
	PBNISubclass();
	PBNISubclass( IPB_Session * pSession, pbobject obj );
	virtual ~PBNISubclass();

	

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
		mid_Attach = 0,
		mid_Detach = 1,
		// TODO: add enum entries for each callable method
		NO_MORE_METHODS
	};


protected:
 	// methods callable from PowerBuilder
	PBXRESULT Attach( PBCallInfo * ci, pbobject obj);
	PBXRESULT Detach( PBCallInfo * ci, pbobject obj );

};

LRESULT APIENTRY SubClassProc(
    HWND hwndDlg, 
    UINT uMsg, 
    WPARAM wParam, 
    LPARAM lParam);

LPCTSTR GetPBClassPropID(long index);

#endif	// !defined(PBNISubclass_H)