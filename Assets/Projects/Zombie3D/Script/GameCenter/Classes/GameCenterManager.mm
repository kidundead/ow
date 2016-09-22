
#import "GameCenterManager.h"
#import "GameCenterViewController.h"

void UnityPause(bool pause);
extern "C" UIViewController * g_ViewController;

@implementation GameCenterManager

@synthesize login_status;
@synthesize score_best;
@synthesize score_status;
@synthesize achievement_best;
@synthesize achievement_status;

- (id)init
{
	self = [super init];

	if (self != NULL)
	{
		login_status = 0;
	}

	return self;
}

- (void)dealloc
{
	[super dealloc];
}

- (void)callDelegate: (SEL) selector withArg1: (id) arg1 withArg2: (id) arg2
{
	assert([NSThread isMainThread]);
	if ([self respondsToSelector: selector])
	{
		if (arg2 != NULL)
		{
			[self performSelector: selector withObject: arg1 withObject: arg2];
		}
		else
		{
			[self performSelector: selector withObject: arg1];
		}
	}
	else
	{
		NSLog(@"Missed Method");
	}
}

- (void)callDelegateOnMainThread: (SEL) selector withArg1: (id) arg1 withArg2: (id) arg2
{
	dispatch_async(dispatch_get_main_queue(), ^(void)
	{
		[self callDelegate: selector withArg1: arg1 withArg2: arg2];
	});
}

- (bool)IsSupported
{
	// check for presence of GKLocalPlayer API
	Class gcClass = (NSClassFromString(@"GKLocalPlayer"));

	// check if the device is running iOS 4.1 or later
	NSString* reqSysVer = @"4.1";
	NSString* currSysVer = [[UIDevice currentDevice] systemVersion];
	BOOL osVersionSupported = ([currSysVer compare:reqSysVer options:NSNumericSearch] != NSOrderedAscending);

	//
	BOOL res = gcClass && osVersionSupported;
	return (res == YES);
}

- (bool)IsLogin
{
	return ([GKLocalPlayer localPlayer].authenticated == YES);
}

- (bool)Login
{
	if ([GKLocalPlayer localPlayer].authenticated == YES)
	{
		return YES;
	}

	if (login_status == 1)
	{
		return YES;
	}

	login_status = 1;
	[[GKLocalPlayer localPlayer] authenticateWithCompletionHandler:^(NSError *error) 
	{
		[self callDelegateOnMainThread: @selector(OnLoginResult:) withArg1: error withArg2: NULL];
	}];

	return true;
}

- (void)OnLoginResult:(NSError*)error
{
	if (error == NULL)
	{
		NSLog(@"OnLoginResult OK");
		login_status = 2;
	}
	else
	{
		NSLog(@"OnLoginResult Error: %@", error);
		login_status = 3;
	}
}

- (int)LoginStatus
{
	return login_status;
}

- (bool)GetAccount:(char*)buf buf_len:(int)buf_len
{
	if ([GKLocalPlayer localPlayer].authenticated == NO)
	{
		return false;
	}

	const char* p = [[GKLocalPlayer localPlayer].playerID cStringUsingEncoding:NSUTF8StringEncoding];
	strcpy(buf, p);

	return true;
}

- (bool)GetName:(char*)buf buf_len:(int)buf_len
{
	if ([GKLocalPlayer localPlayer].authenticated == NO)
	{
		return false;
	}

	const char* p = [[GKLocalPlayer localPlayer].alias cStringUsingEncoding:NSUTF8StringEncoding];
	strcpy(buf, p);

	return true;
}

- (bool)SubmitScore:(NSString*)category score:(int)score
{
	//
	if ([GKLocalPlayer localPlayer].authenticated == NO)
	{
		return false;
	}

	//
	std::string category_str = [category cStringUsingEncoding:NSUTF8StringEncoding];
	std::map<std::string, int>::iterator iter = score_best.find(category_str);
	if (iter != score_best.end())
	{
		int best = iter->second;
		if (best >= score)
		{
			return false;
		}
	}

	//
	NSString* session = [NSString stringWithFormat:@"%@|%d", category, score];
	std::string session_str = [category cStringUsingEncoding:NSUTF8StringEncoding];
	score_status[session_str] = 1;

	//
	GKScore* gk_score = [[[GKScore alloc] initWithCategory:category] autorelease];
	gk_score.value = score;
	[gk_score reportScoreWithCompletionHandler: ^(NSError *error)
	{
		[self callDelegateOnMainThread: @selector(OnSubmitScoreResult:session:) withArg1: error withArg2: session];
	}];

	return true;
}

- (void)OnSubmitScoreResult:(NSError*)error session:(NSString*)session
{
	std::string session_str = [session cStringUsingEncoding:NSUTF8StringEncoding];
	if (error == NULL)
	{
		score_status[session_str] = 2;

		std::string::size_type pos = session_str.find("|");
		if (pos != std::string::npos)
		{
			std::string category = session_str.substr(0, pos);
			std::string temp = session_str.substr(pos + 1);
			int score = atoi(temp.c_str());

			if (score_best[category] < score)
			{
				score_best[category] = score;
			}
		}

		NSLog(@"OnSubmitScoreResult OK");
	}
	else
	{
		score_status[session_str] = 3;

		NSLog(@"OnSubmitScoreResult Error: %@", error);
	}
}

- (int)SubmitScoreStatus:(NSString*)category score:(int)score
{
	NSString* session = [NSString stringWithFormat:@"%@|%d", category, score];
	std::string session_str = [session cStringUsingEncoding:NSUTF8StringEncoding];
	std::map<std::string, int>::iterator iter = score_status.find(session_str);
	if (iter == score_status.end())
	{
		return 0;
	}
	else
	{
		return iter->second;
	}
}

- (bool)SubmitAchievement:(NSString*)category percent:(int)percent
{
	//
	if ([GKLocalPlayer localPlayer].authenticated == NO)
	{
		return false;
	}

	//
	std::string category_str = [category cStringUsingEncoding:NSUTF8StringEncoding];
	std::map<std::string, int>::iterator iter = achievement_best.find(category_str);
	if (iter != achievement_best.end())
	{
		int best = iter->second;
		if (best >= percent)
		{
			return false;
		}
	}

	//
	NSString* session = [NSString stringWithFormat:@"%@|%d", category, percent];
	std::string session_str = [category cStringUsingEncoding:NSUTF8StringEncoding];
	achievement_status[session_str] = 1;

	//
	GKAchievement* achievement = [[GKAchievement alloc] initWithIdentifier:category];
	achievement.percentComplete = percent;
	[achievement reportAchievementWithCompletionHandler: ^(NSError *error)
	{
		[self callDelegateOnMainThread: @selector(OnSubmitAchievementResult:session:) withArg1: error withArg2: session];
	}];

	return true;
}

- (void)OnSubmitAchievementResult:(NSError*)error session:(NSString*)session
{
	std::string session_str = [session cStringUsingEncoding:NSUTF8StringEncoding];
	if (error == NULL)
	{
		achievement_status[session_str] = 2;

		std::string::size_type pos = session_str.find("|");
		if (pos != std::string::npos)
		{
			std::string category = session_str.substr(0, pos);
			std::string temp = session_str.substr(pos + 1);
			int percent = atoi(temp.c_str());

			if (achievement_best[category] < percent)
			{
				achievement_best[category] = percent;
			}
		}

		NSLog(@"OnSubmitAchievementResult OK");
	}
	else
	{
		achievement_status[session_str] = 3;

		NSLog(@"OnSubmitAchievementResult Error: %@", error);
	}
}

- (int)SubmitAchievementStatus:(NSString*)category percent:(int)percent
{
	NSString* session = [NSString stringWithFormat:@"%@|%d", category, percent];
	std::string session_str = [session cStringUsingEncoding:NSUTF8StringEncoding];
	std::map<std::string, int>::iterator iter = achievement_status.find(session_str);
	if (iter == achievement_status.end())
	{
		return 0;
	}
	else
	{
		return iter->second;
	}
}

- (bool)OpenLeaderboard
{
	//
	if ([GKLocalPlayer localPlayer].authenticated == NO)
	{
		return false;
	}
	
	UnityPause(true);
	//
	UIViewController* ee_view_controller = g_ViewController; //[UIApplication sharedApplication].keyWindow.rootViewController;

	NSLog(@"OpenLeaderboard");

	//
	GKLeaderboardViewControllerExt* viewController = [[GKLeaderboardViewControllerExt alloc] init];
	if (viewController != NULL)
	{
		viewController.category = nil;
		viewController.timeScope = GKLeaderboardTimeScopeAllTime;
		viewController.leaderboardDelegate = self;

		[ee_view_controller presentModalViewController:viewController animated:YES];
	}

	return true;
}

- (bool)OpenLeaderboardWithCategory:(NSString*)category
{
	//
	if ([GKLocalPlayer localPlayer].authenticated == NO)
	{
		return false;
	}

	UnityPause(true);
	//
	UIViewController* ee_view_controller = g_ViewController; //[UIApplication sharedApplication].keyWindow.rootViewController;

	NSLog(@"OpenLeaderboard: %@", category);

	//
	GKLeaderboardViewControllerExt* viewController = [[GKLeaderboardViewControllerExt alloc] init];
	if (viewController != NULL) 
	{
		viewController.category = category;
		viewController.timeScope = GKLeaderboardTimeScopeAllTime;
		viewController.leaderboardDelegate = self;

		[ee_view_controller presentModalViewController:viewController animated:YES];
	}

	return true;
}

- (void)leaderboardViewControllerDidFinish:(GKLeaderboardViewController*)viewController
{
	NSLog(@"CloseLeaderboard");

	UnityPause(false);
	[viewController dismissModalViewControllerAnimated: YES];
	[viewController release];
}

- (bool)OpenAchievement
{
	//
	if ([GKLocalPlayer localPlayer].authenticated == NO)
	{
		NSLog(@"OpenAchievement return false");
		
		return false;
	}

	UnityPause(true);
	//
	UIViewController* ee_view_controller = g_ViewController;//[UIApplication sharedApplication].keyWindow.rootViewController;

	//
	GKAchievementViewControllerExt* viewController = [[GKAchievementViewControllerExt alloc] init];
	if (viewController != NULL)
	{
		viewController.achievementDelegate = self;

		[ee_view_controller presentModalViewController:viewController animated:YES];
	}

	return true;
}

- (void)achievementViewControllerDidFinish:(GKAchievementViewController *)viewController;
{
	NSLog(@"CloseAchievement");

	UnityPause(false);
	[viewController dismissModalViewControllerAnimated: YES];
	[viewController release];
	
}


@end

