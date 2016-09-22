
#ifndef _GAME_CENTER_VIEW_CONTROLLER_H_
#define _GAME_CENTER_VIEW_CONTROLLER_H_


#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import <GameKit/GameKit.h>


@interface GKLeaderboardViewControllerExt : GKLeaderboardViewController
{
	bool _supportedPortrait;
	bool _supportedPortraitUpsideDown;
	bool _supportedLandscapeLeft;
	bool _supportedLandscapeRight;
}

@end


@interface GKAchievementViewControllerExt : GKAchievementViewController
{
	bool _supportedPortrait;
	bool _supportedPortraitUpsideDown;
	bool _supportedLandscapeLeft;
	bool _supportedLandscapeRight;
}

@end


#endif // _GAME_CENTER_VIEW_CONTROLLER_H_
