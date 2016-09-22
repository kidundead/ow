
#ifndef _GAME_CENTER_MANAGER_H_
#define _GAME_CENTER_MANAGER_H_


#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import <GameKit/GameKit.h>
#include <string>
#include <map>


@protocol GameCenterManagerDelegate<NSObject>
- (void)OnLoginResult:(NSError*)error;
- (void)OnSubmitScoreResult:(NSError*)error session:(NSString*)session;
- (void)OnSubmitAchievementResult:(NSError*)error session:(NSString*)session;
@end


@interface GameCenterManager : NSObject<GameCenterManagerDelegate, GKLeaderboardViewControllerDelegate, GKAchievementViewControllerDelegate>
{
	int login_status;
	std::map<std::string, int> score_best;
	std::map<std::string, int> score_status;
	std::map<std::string, int> achievement_best;
	std::map<std::string, int> achievement_status;
}

@property (nonatomic) int login_status;
@property (nonatomic) std::map<std::string, int> score_best;
@property (nonatomic) std::map<std::string, int> score_status;
@property (nonatomic) std::map<std::string, int> achievement_best;
@property (nonatomic) std::map<std::string, int> achievement_status;

- (bool)IsSupported;

- (bool)IsLogin;
- (bool)Login;
- (int)LoginStatus;

- (bool)GetAccount:(char*)buf buf_len:(int)buf_len;
- (bool)GetName:(char*)buf buf_len:(int)buf_len;

- (bool)SubmitScore:(NSString*)category score:(int)score;
- (int)SubmitScoreStatus:(NSString*)category score:(int)score;

- (bool)SubmitAchievement:(NSString*)category percent:(int)percent;
- (int)SubmitAchievementStatus:(NSString*)category percent:(int)percent;

- (bool)OpenLeaderboard;
- (bool)OpenLeaderboardWithCategory:(NSString*)category;
- (bool)OpenAchievement;

@end


#endif // _GAME_CENTER_MANAGER_H_
