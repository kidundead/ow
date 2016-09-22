
#include "GameCenterImpl.h"
#import "GameCenterManager.h"


// class GameCenterImpl
GameCenterImpl::GameCenterImpl()
{
	m_is_supported = 0;
	m_manager = [[GameCenterManager alloc] init];
}

GameCenterImpl::~GameCenterImpl()
{
	m_is_supported = 0;
	// [m_manager release]; NOTICE not release here, just let it leak
	m_manager = nil;
}

bool GameCenterImpl::IsSupported()
{
	if (m_is_supported == 0)
	{
		bool supported = [m_manager IsSupported];
		if (supported)
		{
			m_is_supported = 1;
		}
		else
		{
			m_is_supported = -1;
		}
	}

	if (m_is_supported == 1)
	{
		return true;
	}
	else
	{
		return false;
	}
}

bool GameCenterImpl::IsLogin()
{
	bool result = [m_manager IsLogin];
	return result;
}

bool GameCenterImpl::Login()
{
	bool result = [m_manager Login];
	return result;
}

Triniti2D::GameCenter::LOGIN_STATUS GameCenterImpl::LoginStatus()
{
	int result = [m_manager LoginStatus];
	return (Triniti2D::GameCenter::LOGIN_STATUS)result;
}

bool GameCenterImpl::GetAccount(std::string& account)
{
	char buf[1024];
	bool result = [m_manager GetAccount:buf buf_len:1023];
	if (result)
	{
		account = buf;
	}

	return result;
}

bool GameCenterImpl::GetName(std::string& name)
{
	char buf[1024];
	bool result = [m_manager GetName:buf buf_len:1023];
	if (result)
	{
		name = buf;
	}

	return result;
}

bool GameCenterImpl::SubmitScore(const std::string& category, int score)
{
	NSString* ns_category = [NSString stringWithCString:category.c_str() encoding:NSUTF8StringEncoding];
	bool result = [m_manager SubmitScore:ns_category score:score];
	return result;
}

Triniti2D::GameCenter::SUBMIT_STATUS GameCenterImpl::SubmitScoreStatus(const std::string& category, int score)
{
	NSString* ns_category = [NSString stringWithCString:category.c_str() encoding:NSUTF8StringEncoding];
	int result = [m_manager SubmitScoreStatus:ns_category score:score];
	return (Triniti2D::GameCenter::SUBMIT_STATUS)result;
}

bool GameCenterImpl::SubmitAchievement(const std::string& category, int percent)
{
	NSString* ns_category = [NSString stringWithCString:category.c_str() encoding:NSUTF8StringEncoding];
	bool result = [m_manager SubmitAchievement:ns_category percent:percent];
	return result;
}

Triniti2D::GameCenter::SUBMIT_STATUS GameCenterImpl::SubmitAchievementStatus(const std::string& category, int percent)
{
	NSString* ns_category = [NSString stringWithCString:category.c_str() encoding:NSUTF8StringEncoding];
	int result = [m_manager SubmitAchievementStatus:ns_category percent:percent];
	return (Triniti2D::GameCenter::SUBMIT_STATUS)result;
}

bool GameCenterImpl::OpenLeaderboard()
{
	bool result = [m_manager OpenLeaderboard];
	return result;
}

bool GameCenterImpl::OpenLeaderboardWithCategory(const std::string& category)
{
	NSString* ns_category = [NSString stringWithCString:category.c_str() encoding:NSUTF8StringEncoding];
	bool result = [m_manager OpenLeaderboardWithCategory:ns_category];
	return result;
}

bool GameCenterImpl::OpenAchievement()
{
	bool result = [m_manager OpenAchievement];
	return result;
}

