#include "GameCenter.h"

using namespace Triniti2D;

extern "C" void GMInitialize()
{
	GameCenter::Initialize();
}

extern "C" void GMUninitialize()
{
	GameCenter::Uninitialize();
}

extern "C" bool GMIsSupported()
{
	return GameCenter::IsSupported();
}

extern "C" bool GMIsLogin()
{
	return GameCenter::IsLogin();
}

extern "C" bool GMLogin()
{
	return GameCenter::Login();
}

extern "C" int GMLoginStatus()
{
	return (int)GameCenter::LoginStatus();
}

extern "C" bool GMGetAccount(char value[1024])
{
	std::string buf;
	bool bRet = GameCenter::GetAccount(buf);
	if(bRet)
	{
		strncpy(value, buf.c_str(), 1024) [1024] = 0;
	}
	return bRet;
}

extern "C" bool GMGetName(char value[1024])
{
	std::string buf;
	bool bRet = GameCenter::GetName(buf);
	if(bRet)
	{
		strncpy(value, buf.c_str(), 1024) [1024] = 0;
	}
	return bRet;
}

extern "C" bool GMSubmitScore(const char* category, int score)
{
	return GameCenter::SubmitScore(category, score);
}

extern "C" int GMSubmitScoreStatus(const char* category, int score)
{
	return (int)GameCenter::SubmitScoreStatus(category, score);
}

extern "C" bool	GMSubmitAchievement(const char* category, int percent)
{
	return GameCenter::SubmitAchievement(category, percent);
}

extern "C" int	GMSubmitAchievementStatus(const char* category, int percent)
{
	return (int)GameCenter::SubmitAchievementStatus(category, percent);
}

extern "C" bool GMOpenLeaderboard()
{
	return GameCenter::OpenLeaderboard();
}

extern "C" bool GMOpenLeaderboardForCategory(const char* category)
{
	return GameCenter::OpenLeaderboard(category);
}

extern "C" bool GMOpenAchievement()
{
	return GameCenter::OpenAchievement();
}