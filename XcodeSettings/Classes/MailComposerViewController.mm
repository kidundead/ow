/*
 
 copyright 2009-10-27
 by trinitigame
 changzhidong created
 
 */

#import "MailComposerViewController.h"

@implementation MailComposerViewController


@synthesize picker;
@synthesize _oritentation;
-(void)sendEmail:(NSString *)subject Recipt:(NSString *)recipt Body:(NSString *)emailBody;
{
	Class mailClass = (NSClassFromString(@"MFMailComposeViewController"));
	if (mailClass != nil)
	{
		// We must always check whether the current device is configured for sending emails
		if ([mailClass canSendMail])
		{
			[self displayComposerSheet:subject Recipt:recipt Body:emailBody];
		}
		else
		{
			[self launchMailAppOnDevice:(NSString *)subject Recipt:(NSString *)recipt  Body:(NSString *)emailBody];
		}
	}
	else
	{
		[self launchMailAppOnDevice:(NSString *)subject Recipt:(NSString *)recipt  Body:(NSString *)emailBody];
	}
	
}

#pragma mark -
#pragma mark Compose Mail

// Displays an email composition interface inside the application. Populates all the Mail fields. 
-(void)displayComposerSheet:(NSString *)subject Recipt:(NSString *)recipt Body:(NSString *)emailBody; 
{
	_oritentation = [UIApplication sharedApplication].statusBarOrientation;
	[[UIApplication sharedApplication] setStatusBarOrientation:UIInterfaceOrientationPortrait];
	
	picker = [[MFMailComposeViewController alloc] init];

	picker.mailComposeDelegate = self;
	
	[picker setSubject:subject];
	

	// Set up recipients
	//NSArray *toRecipients = [NSArray arrayWithObject:recipt]; 
	if(recipt.length !=0)
	{
		NSArray *toRecipients = [NSArray arrayWithObject:recipt]; 
		[picker setToRecipients:toRecipients];
	}
//NSArray *ccRecipients = [NSArray arrayWithObjects:@"second@example.com", @"third@example.com", nil]; 
	//NSArray *bccRecipients = [NSArray arrayWithObject:@"fourth@example.com"]; 
	
//	[picker setToRecipients:toRecipients];
	//[picker setCcRecipients:ccRecipients];	
	//[picker setBccRecipients:bccRecipients];
	
	
	// Fill out the email body text

	[picker setMessageBody:emailBody isHTML:YES];

	
	
	//[self presentModalViewController:picker animated:YES];
	UIWindow* window = [UIApplication sharedApplication].keyWindow;
	if (!window) {
		window = [[UIApplication sharedApplication].windows objectAtIndex:0];
	}

	[window addSubview:picker.view];
	
}


// Dismisses the email composition interface when users tap Cancel or Send. Proceeds to update the message field with the result of the operation.
- (void)mailComposeController:(MFMailComposeViewController*)controller didFinishWithResult:(MFMailComposeResult)result error:(NSError*)error 
{	
	if( _oritentation != UIInterfaceOrientationPortrait)
		[[UIApplication sharedApplication] setStatusBarOrientation:UIInterfaceOrientationLandscapeRight];
	[picker.view removeFromSuperview];
	[picker release];
	//mailView=nil;
}


#pragma mark -
#pragma mark Workaround

// Launches the Mail application on the device.
-(void)launchMailAppOnDevice:(NSString *)subject Recipt:(NSString *)recipt  Body:(NSString *)emailBody;
{
	//NSString *recipients = @"mailto:first@example.com?cc=second@example.com,third@example.com&subject=Hello from California!";
	//NSString *body = @"&body=It is raining in sunny California!";
	
	NSString *email = [NSString stringWithFormat:@"mailto:%@&subject=%@&body=%@", recipt, subject,emailBody];
	email = [email stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding];
	
	[[UIApplication sharedApplication] openURL:[NSURL URLWithString:email]];
}


#pragma mark -
#pragma mark Unload views

- (void)viewDidUnload 
{
	// Release any retained subviews of the main view.
	// e.g. self.myOutlet = nil;
	//self.message = nil;
}

#pragma mark -
#pragma mark Memory management

- (void)dealloc 
{
  //  [message release];
	//[mailView release];
	[super dealloc];
}

@end
