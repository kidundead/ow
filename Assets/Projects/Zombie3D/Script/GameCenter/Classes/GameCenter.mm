
#include "GameCenter.h"
#include "GameCenterImpl.h"
#include <assert.h>


namespace Triniti2D {


// class GameCenter
GameCenterImpl* GameCenter::m_impl = NULL;

void GameCenter::Initialize()
{
	if (m_impl == NULL)
	{
		m_impl = new ::GameCenterImpl;
	}
}

void GameCenter::Uninitialize()
{
	if (m_impl != NULL)
	{
		delete m_impl;
		m_impl = NULL;
	}
}

bool GameCenter::IsSupported()
{
	if (m_impl == NULL)
	{
		return false;
	}

	return m_impl->IsSupported();
}

bool GameCenter::IsLogin()
{
	if (!IsSupported())
	{
		return false;
	}

	return m_impl->IsLogin();
}

bool GameCenter::Login()
{
	if (!IsSupported())
	{
		return false;
	}

	return m_impl->Login();
}

GameCenter::LOGIN_STATUS GameCenter::LoginStatus()
{
	if (!IsSupported())
	{
		return LOGIN_STATUS_IDLE;
	}

	return m_impl->LoginStatus();
}

bool GameCenter::GetAccount(std::string& account)
{
	if (!IsSupported())
	{
		return false;
	}

	return m_impl->GetAccount(account);
}

bool GameCenter::GetName(std::string& name)
{
	if (!IsSupported())
	{
		return false;
	}

	return m_impl->GetName(name);
}

bool GameCenter::SubmitScore(const std::string& category, int score)
{
	if (!IsSupported())
	{
		return false;
	}

	return m_impl->SubmitScore(category, score);
}

GameCenter::SUBMIT_STATUS GameCenter::SubmitScoreStatus(const std::string& category, int score)
{
	if (!IsSupported())
	{
		return SUBMIT_STATUS_IDLE;
	}

	return m_impl->SubmitScoreStatus(category, score);
}

bool GameCenter::SubmitAchievement(const std::string& category, int percent)
{
	if (!IsSupported())
	{
		return false;
	}

	return m_impl->SubmitAchievement(category, percent);
}

GameCenter::SUBMIT_STATUS GameCenter::SubmitAchievementStatus(const std::string& category, int percent)
{
	if (!IsSupported())
	{
		return SUBMIT_STATUS_IDLE;
	}

	return m_impl->SubmitAchievementStatus(category, percent);
}

bool GameCenter::OpenLeaderboard()
{
	if (!IsSupported())
	{
		return false;
	}

	return m_impl->OpenLeaderboard();
}

bool GameCenter::OpenLeaderboard(const std::string& category)
{
	if (!IsSupported())
	{
		return false;
	}

	return m_impl->OpenLeaderboardWithCategory(category);
}

bool GameCenter::OpenAchievement()
{
	if (!IsSupported())
	{
		return false;
	}

	return m_impl->OpenAchievement();
}


} // namespace Triniti2D

