//
//  MessageBoxDelegate.m
//  MessageBox
//
//  Created by chengpu on 6/21/10.
//  Copyright __MyCompanyName__ 2010. All rights reserved.
//

#import "MessageBoxDelegate.h"


int OpenMessageBox(const char* title, const char* message, int button_number, const char* button_name[])
{
	//
	UIInterfaceOrientation orientation = [UIApplication sharedApplication].statusBarOrientation;
	[[UIApplication sharedApplication] setStatusBarOrientation:orientation];

	//
	MessageBoxDelegate *delegate = [[MessageBoxDelegate alloc] init];

	//
	NSString* string = nil;
	if (button_number <= 0)
	{
		string = @"OK";
	}
	else
	{
		string = [[NSString alloc] initWithCString:button_name[0] encoding:NSUTF8StringEncoding];
	}

	UIAlertView* Alert = [[UIAlertView alloc] initWithTitle:[NSString stringWithUTF8String:title] message:[NSString stringWithUTF8String:message] delegate:delegate cancelButtonTitle:string otherButtonTitles:nil];

	NSString* current = nil;
	for (int i = 1; i < button_number; i++)
	{
		current = [[NSString alloc] initWithCString:button_name[i] encoding:NSUTF8StringEncoding];
		[Alert addButtonWithTitle:current];
	}

	[Alert show];

	while (Alert.visible)
	{
		[[NSRunLoop currentRunLoop] runMode: NSDefaultRunLoopMode beforeDate:[NSDate dateWithTimeIntervalSinceNow: 0.100]];
	}

	[Alert release];

	int index = delegate->_buttonNumber;

	[delegate release];

	return index;
}


@implementation MessageBoxDelegate

@synthesize _buttonNumber;

- (void)alertView:(UIAlertView*)alertView clickedButtonAtIndex:(NSInteger)buttonIndex
{
	self._buttonNumber = buttonIndex;
}

- (void)didPresentAlertView:(UIAlertView*)alertView
{
	// UIAlertView in landscape mode
	//[UIView beginAnimations:@"" context:nil];
	//[UIView setAnimationDuration:0.1];
	//alertView.transform = CGAffineTransformRotate(alertView.transform, 3.14159/2);
	//[UIView commitAnimations];

	// orientation
	/*
	UIInterfaceOrientation orientation = [UIApplication sharedApplication].statusBarOrientation;
	if (orientation == UIInterfaceOrientationLandscapeLeft)
	{
		alertView.transform = CGAffineTransformRotate(alertView.transform, -M_PI / 2);
	}
	else if (orientation == UIInterfaceOrientationLandscapeRight)
	{
		alertView.transform = CGAffineTransformRotate(alertView.transform, M_PI / 2);
	}
	else if (orientation == UIInterfaceOrientationPortraitUpsideDown)
	{
		alertView.transform = CGAffineTransformRotate(alertView.transform, M_PI);
	}
	else // if (orientation == UIInterfaceOrientationPortrait)
	{
	}
	*/
} 

@end

