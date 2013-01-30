// PBNISubclass.cpp : defines the entry point for the PBNI DLL.
//Global: 0
//Unicode: -1
#include "PBNIKodigo.h"
#include "PBNIHook.h"
#include "PBNISubclass.h"
#include "PBNICanvas.h"
#include "PBNITimer.h"
//#include "PBNIColor.h"

BOOL APIENTRY DllMain
(
   HANDLE hModule,
   DWORD ul_reason_for_all,
   LPVOID lpReserved
)
{	
	switch(ul_reason_for_all)
	{	
	case DLL_PROCESS_ATTACH:
		PBNICanvas::RegisterClass();
		PBNICanvas::SetDLLHandle(hModule);
		break;
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:		
		break;
	case DLL_PROCESS_DETACH:
		PBNICanvas::UnregisterClass();
		break;
	}
			
	return TRUE;
}

// The description returned from PBX_GetDescription is used by
// the PBX2PBD tool to create pb groups for the PBNI class.
//
// + The description can contain more than one class definition.
// + A class definition can reference any class definition that
//   appears before it in the description.
// + The PBNI class must inherit from a class that inherits from
//   NonVisualObject, such as Transaction or Exception

PBXEXPORT LPCTSTR PBXCALL PBX_GetDescription()
{
   // combined class description
   static const TCHAR classDesc[] = {
      /* PBNISubclass */
      _T("class n_pbni_hook from nonvisualobject\n") \
      _T("   subroutine of_register(long hwnd)\n") \
      _T("   subroutine of_attachwndproc()\n") \
      _T("   subroutine of_attachmouseproc()\n") \
      _T("   subroutine of_attachkeyboardproc()\n") \
      _T("   subroutine of_attachmessageproc()\n") \
      _T("   function int of_wndproc(long hwnd, long msg, ulong wparam, ulong lparam)\n") \
      _T("   function int of_mouseproc(long hwnd, long px, long py, long mousemessage, long hittest)\n") \
      _T("   function int of_keyboardproc(long vkcode, ulong flags)\n") \
      _T("   function int of_msgfilterproc(long code, long hwnd, long msg, ulong wparam, ulong lparam, long ptime, integer px, integer py)\n") \
      _T("   subroutine of_detach()\n") \
      _T("end class\n")\
	  _T("class n_pbni_subclass from nonvisualobject\n") \
      _T("   subroutine of_attach(long hwnd)\n") \
      _T("   subroutine of_detach(long hwnd)\n") \
      _T("   function long of_wndproc(long hwnd, long msg, ulong wparam, ulong lparam)\n") \
      _T("   function long of_postprocessquery(long msg)\n") \
      _T("end class\n") \
	  _T("class n_pbni_timer from nonvisualobject\n") \
      _T("   function long of_start(long interval)\n") \
      _T("   subroutine of_stop(long timerid)\n") \
      _T("   subroutine of_pulse()\n") \
      _T("end class\n") \
	  _T("class u_pbni_canvas from userobject\n") \
	  _T("   event int onpaint(ulong hdc)\n") \
	  _T("end class\n")
   };

   return (LPCTSTR)classDesc;
}

PBXEXPORT PBXRESULT PBXCALL PBX_CreateNonVisualObject
(
   IPB_Session * session,
   pbobject obj,
   LPCTSTR className,
   IPBX_NonVisualObject ** nvobj
)
{
   // The name must not contain upper case
   if (_tcscmp(className, _T("n_pbni_subclass")) == 0)
      *nvobj = new PBNISubclass(session, obj);
   if (_tcscmp(className, _T("n_pbni_hook")) == 0)
      *nvobj = new PBNIHook(session, obj);
   if (_tcscmp(className, _T("n_pbni_timer")) == 0)
      *nvobj = new PBNITimer(session, obj);
   //if (_tcscmp(className, _T("n_pbni_color")) == 0)
   //   *nvobj = new PBNIColor(session, obj);

   return PBX_OK;
}

PBXEXPORT PBXRESULT PBXCALL PBX_CreateVisualObject
(
	IPB_Session*			pbsession, 
	pbobject				pbobj,
	LPCTSTR					className,		
	IPBX_VisualObject	**obj
)
{
	PBXRESULT result = PBX_OK;

	if (_tcscmp(className, _T("u_pbni_canvas")) == 0)
	{
		*obj = new PBNICanvas(pbsession, pbobj);
	}
	else
	{
		*obj = NULL;
		result = PBX_FAIL;
	}

	return PBX_OK;
};
