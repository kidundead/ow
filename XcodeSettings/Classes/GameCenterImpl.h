
#ifndef _GAME_CENTER_IMPL_H_
#define _GAME_CENTER_IMPL_H_


#include "GameCenter.h"
#include <string>


@class GameCenterManager;


//! @class GameCenterImpl
//! @brief GameCenterImpl
class GameCenterImpl
{
public:
	GameCenterImpl();
	~GameCenterImpl();

public:
	bool IsSupported();

public:
	bool IsLogin();
	bool Login();
	Triniti2D::GameCenter::LOGIN_STATUS LoginStatus();

public:
	bool GetAccount(std::string& account);
	bool GetName(std::string& name);

public:
	bool SubmitScore(const std::string& category, int score);
	Triniti2D::GameCenter::SUBMIT_STATUS SubmitScoreStatus(const std::string& category, int score);
	bool SubmitAchievement(const std::string& category, int percent);
	Triniti2D::GameCenter::SUBMIT_STATUS SubmitAchievementStatus(const std::string& category, int percent);

public:
	bool OpenLeaderboard();
	bool OpenLeaderboardWithCategory(const std::string& category);
	bool OpenAchievement();

private:
	int m_is_supported;
	GameCenterManager* m_manager;
};


#endif // _GAME_CENTER_IMPL_H_
