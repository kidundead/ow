
#import "GameCenterViewController.h"


@implementation GKLeaderboardViewControllerExt

// Implement loadView to create a view hierarchy programmatically, without using a nib.
- (void)loadView
{
	[super loadView];

	/*
	_supportedPortrait = false;
	_supportedPortraitUpsideDown = false;
	_supportedLandscapeLeft = false;
	_supportedLandscapeRight = false;

	NSArray* supportedOrientations = [[[NSBundle mainBundle] infoDictionary] objectForKey:@"UISupportedInterfaceOrientations"];
	if (supportedOrientations != nil)
	{
		for(int i = 0; i < [supportedOrientations count]; i++)
		{
			NSString* orientation = (NSString*)[supportedOrientations objectAtIndex:i];
			if ([orientation isEqualToString:@"UIInterfaceOrientationPortrait"])
			{
				_supportedPortrait = true;
				printf("UISupportedInterfaceOrientations: UIInterfaceOrientationPortrait\n");
			}
			if ([orientation isEqualToString:@"UIInterfaceOrientationPortraitUpsideDown"])
			{
				_supportedPortraitUpsideDown = true;
				printf("UISupportedInterfaceOrientations: UIInterfaceOrientationPortraitUpsideDown\n");
			}
			if ([orientation isEqualToString:@"UIInterfaceOrientationLandscapeLeft"])
			{
				_supportedLandscapeLeft = true;
				printf("UISupportedInterfaceOrientations: UIInterfaceOrientationLandscapeLeft\n");
			}
			if ([orientation isEqualToString:@"UIInterfaceOrientationLandscapeRight"])
			{
				_supportedLandscapeRight = true;
				printf("UISupportedInterfaceOrientations: UIInterfaceOrientationLandscapeRight\n");
			}
		}
	}

	// orientation
	UIInterfaceOrientation orientation = [UIApplication sharedApplication].statusBarOrientation;
	if (orientation == UIInterfaceOrientationLandscapeLeft)
	{
		_supportedLandscapeLeft = true;
	}
	else if (orientation == UIInterfaceOrientationLandscapeRight)
	{
		_supportedLandscapeRight = true;
	}
	else if (orientation == UIInterfaceOrientationPortraitUpsideDown)
	{
		_supportedPortraitUpsideDown = true;
	}
	else // if (orientation == UIInterfaceOrientationPortrait)
	{
		_supportedPortrait = true;
	}
	 */
}

// Override to allow orientations other than the default portrait orientation.
- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation
{
/*
	if (interfaceOrientation == UIInterfaceOrientationPortrait)
	{
		return _supportedPortrait ? YES : NO;
	}
	else if (interfaceOrientation == UIInterfaceOrientationPortraitUpsideDown)
	{
		return _supportedPortraitUpsideDown ? YES : NO;
	}
	else if (interfaceOrientation == UIInterfaceOrientationLandscapeRight)
	{
		return _supportedLandscapeRight ? YES : NO;
	}
	else if (interfaceOrientation == UIInterfaceOrientationLandscapeLeft)
	{
		return _supportedLandscapeLeft ? YES : NO;
	}
*/
	if (interfaceOrientation == UIInterfaceOrientationLandscapeRight)
	{
		return YES;
	}
	return NO;
}

@end


@implementation GKAchievementViewControllerExt

// Implement loadView to create a view hierarchy programmatically, without using a nib.

- (void)loadView
{
	[super loadView];

	/*
	_supportedPortrait = false;
	_supportedPortraitUpsideDown = false;
	_supportedLandscapeLeft = false;
	_supportedLandscapeRight = false;

	NSArray* supportedOrientations = [[[NSBundle mainBundle] infoDictionary] objectForKey:@"UISupportedInterfaceOrientations"];
	if (supportedOrientations != nil)
	{
		for(int i = 0; i < [supportedOrientations count]; i++)
		{
			NSString* orientation = (NSString*)[supportedOrientations objectAtIndex:i];
			if ([orientation isEqualToString:@"UIInterfaceOrientationPortrait"])
			{
				_supportedPortrait = true;
				printf("UISupportedInterfaceOrientations: UIInterfaceOrientationPortrait\n");
			}
			if ([orientation isEqualToString:@"UIInterfaceOrientationPortraitUpsideDown"])
			{
				_supportedPortraitUpsideDown = true;
				printf("UISupportedInterfaceOrientations: UIInterfaceOrientationPortraitUpsideDown\n");
			}
			if ([orientation isEqualToString:@"UIInterfaceOrientationLandscapeLeft"])
			{
				_supportedLandscapeLeft = true;
				printf("UISupportedInterfaceOrientations: UIInterfaceOrientationLandscapeLeft\n");
			}
			if ([orientation isEqualToString:@"UIInterfaceOrientationLandscapeRight"])
			{
				_supportedLandscapeRight = true;
				printf("UISupportedInterfaceOrientations: UIInterfaceOrientationLandscapeRight\n");
			}
		}
	}

	// orientation
	UIInterfaceOrientation orientation = [UIApplication sharedApplication].statusBarOrientation;
	if (orientation == UIInterfaceOrientationLandscapeLeft)
	{
		_supportedLandscapeLeft = true;
	}
	else if (orientation == UIInterfaceOrientationLandscapeRight)
	{
		_supportedLandscapeRight = true;
	}
	else if (orientation == UIInterfaceOrientationPortraitUpsideDown)
	{
		_supportedPortraitUpsideDown = true;
	}
	else // if (orientation == UIInterfaceOrientationPortrait)
	{
		_supportedPortrait = true;
	}
	 */
}
 

// Override to allow orientations other than the default portrait orientation.
- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation
{
/*
	if (interfaceOrientation == UIInterfaceOrientationPortrait)
	{
		return _supportedPortrait ? YES : NO;
	}
	else if (interfaceOrientation == UIInterfaceOrientationPortraitUpsideDown)
	{
		return _supportedPortraitUpsideDown ? YES : NO;
	}
	else if (interfaceOrientation == UIInterfaceOrientationLandscapeRight)
	{
		return _supportedLandscapeRight ? YES : NO;
	}
	else if (interfaceOrientation == UIInterfaceOrientationLandscapeLeft)
	{
		return _supportedLandscapeLeft ? YES : NO;
	}
*/
	
	if (interfaceOrientation == UIInterfaceOrientationLandscapeRight)
	{
		return YES;
	}
	return NO;
}

@end

