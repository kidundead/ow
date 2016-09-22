/*
 
  copyright 2009-10-27
  by trinitigame
  changzhidong created
 
 */

#import <UIKit/UIKit.h>
#import <MessageUI/MessageUI.h>
#import <MessageUI/MFMailComposeViewController.h>

@interface MailComposerViewController : UIViewController <MFMailComposeViewControllerDelegate> 
{
	MFMailComposeViewController *picker;
	UIInterfaceOrientation _oritentation;
}
@property (nonatomic, retain) MFMailComposeViewController *picker;
@property (nonatomic) UIInterfaceOrientation _oritentation;
-(void)sendEmail:(NSString *)subject Recipt:(NSString *)recipt  Body:(NSString *)emailBody;
-(void)displayComposerSheet:(NSString *)subject Recipt:(NSString *)recipt  Body:(NSString *)emailBody;
-(void)launchMailAppOnDevice:(NSString *)subject Recipt:(NSString *)recipt  Body:(NSString *)emailBody;;
@end

