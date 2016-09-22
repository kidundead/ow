
#ifndef _GAME_CENTER_H_
#define _GAME_CENTER_H_


#include <string>


class GameCenterImpl;


namespace Triniti2D {


//! @class GameCenter
//! @brief GameCenter
class GameCenter
{
public:
	//! 登陆状态
	enum LOGIN_STATUS
	{
		LOGIN_STATUS_IDLE = 0,		//!< 空闲
		LOGIN_STATUS_WAIT,			//!< 等待
		LOGIN_STATUS_SUCCESS,		//!< 成功
		LOGIN_STATUS_ERROR			//!< 错误
	};

	//! 提交状态
	enum SUBMIT_STATUS
	{
		SUBMIT_STATUS_IDLE = 0,		//!< 空闲
		SUBMIT_STATUS_WAIT,			//!< 等待
		SUBMIT_STATUS_SUCCESS,		//!< 成功
		SUBMIT_STATUS_ERROR,		//!< 错误
	};

public:
	//! 初始化
	static void Initialize();

	//! 反初始化
	static void Uninitialize();

public:
	//! 当前设备是否支持GameCenter
	static bool IsSupported();

public:
	//! 是否登陆GameCenter
	static bool IsLogin();

	//! 登陆GameCenter
	static bool Login();

	//! 登陆状态
	static LOGIN_STATUS LoginStatus();

public:
	//! 获取帐号
	static bool GetAccount(std::string& account);

	//! 获取名称
	static bool GetName(std::string& name);

public:
	//! 提交分数
	//! @param category 排行榜id
	//! @param score 分数
	static bool SubmitScore(const std::string& category, int score);

	//! 提交分数状态
	//! @param category 排行榜id
	//! @param score 分数
	static SUBMIT_STATUS SubmitScoreStatus(const std::string& category, int score);

	//! 提交成就
	//! @param category 排行榜id
	//! @param percent 百分比
	static bool SubmitAchievement(const std::string& category, int percent);

	//! 提交成就状态
	//! @param category 排行榜id
	//! @param percent 百分比
	static SUBMIT_STATUS SubmitAchievementStatus(const std::string& category, int percent);

public:
	//! 打开排名框
	static bool OpenLeaderboard();

	//! 打开排名框
	static bool OpenLeaderboard(const std::string& category);

	//! 打开成就框
	static bool OpenAchievement();

private:
	//! 实现
	static GameCenterImpl* m_impl;
};


} // namespace Triniti2D


#endif // _GAME_CENTER_H_
