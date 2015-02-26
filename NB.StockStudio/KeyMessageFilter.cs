using System;
using System.Windows.Forms;
/*
 Windows Forms technology is a wrapper on Win32's user32 library. To understand under the hoods of the paint event, you need to know how user32 works.

Message Queue:

Every process in windows has a message queue. When anything happens to a window that belongs to the process, 
 * Windows pushes an event into the message queue of the process. There is a message loop in every application, \
 *that extracts the messages from the queue (by calling GetMessage()) and dispatches the message (calls appropriate function, 
 *called a Window Procedure, by calling DispatchMessage()). So the messages are being processed one after another.
 *It means that when a message is being processed, no other message can be processed.
That's why when you do a time-consuming operation in a form (withoud starting a new thread), 
 * the application stops respongind: You are stuck in processing one message (for example Click event of a button), 
 * so the application cannot process other messages (mouse event, painting events, etc.).

In Windows Forms, Application.Run method runs the message loop of the application. 
 * Messages are passed to Control.WndProc method and this method determines the appropriate OnXxxx method to 
 * call (OnKeyPress, OnMouseMove, OnResize, etc.), and that method raises the respective event (KeyPress, MouseMove, Rezie, etc).

WM_PAINT:

When a program's window needs to be painted (for example when it is first shown or it is restored from minimized state), 
 * Windows queues a WM_PAINT messages into the message queue, only if there is no unprocessed WM_PAINT for the window in the
 * message queue. Also the message loop only extract the WM_PAINT message from the queue only when there is no other message 
 * in the queue. Qoute from the WM_PAINT page in MSDN:

GetMessage returns the WM_PAINT message when there are no other messages in the application's message queue,
 * and DispatchMessage sends the message to the appropriate window procedure.

In Windows Forms, WM_PAINT translates to OnPaint method which raises the Paint event.

When you call Invalidate (which calls the Win32 InvalidateRect function) several times in a single method, 
 * the Paint event still didn't get the chance to be raised. The current event that is being processed must be finished, 
 * also other messages that are sent in the mean time should be processed, then the Paint event is raised.

Please follow the links in the answer and read them thoroughly.
 */
namespace NB.StockStudio
{
	public class KeyMessageFilter : IMessageFilter
	{
		// Methods
		public KeyMessageFilter()
		{
		}

		public bool PreFilterMessage(ref Message m)
		{
			if (m.Msg == 0x100)
			{
				KeyEventArgs args1 = new KeyEventArgs(((Keys) m.WParam.ToInt32()) | Control.ModifierKeys);
				ChartForm.HandleKey(args1);
				if (args1.Handled)
				{
					return args1.Handled;
				}
			}
			return false;
		}


		// Fields
		private const int WM_KEYDOWN = 0x100;
	}
 


}
