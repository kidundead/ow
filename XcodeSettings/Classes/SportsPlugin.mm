
#import "MailComposerViewController.h"
#import "MessageBoxDelegate.h"

extern "C" void SendMail(const char* mailAddress, const char* mailSubject, const char* initContent)
{
	NSString* recipt = [ [NSString alloc] initWithCString:mailAddress encoding:NSUTF8StringEncoding];
	NSString* subject = [ [NSString alloc] initWithCString:mailSubject encoding:NSUTF8StringEncoding];
	NSString *body = [ [NSString alloc] initWithCString:initContent encoding:NSUTF8StringEncoding];
	
	MailComposerViewController *mcvc =[ [MailComposerViewController alloc] init];
	[mcvc sendEmail:subject Recipt:recipt Body:body ];
}

extern "C" int MessgeBox1(const char* title, const char* message, const char* button)
{
	const char* button_name[] = { button };
	return OpenMessageBox(title, message, 1, button_name);
}

extern "C" int MessgeBox2(const char* title, const char* message, const char* button1, const char* button2)
{
	const char* button_name[] = { button1, button2 };
	return OpenMessageBox(title, message, 2, button_name);
}