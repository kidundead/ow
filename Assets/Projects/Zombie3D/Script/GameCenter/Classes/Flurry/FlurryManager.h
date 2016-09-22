
#ifndef _FLURRY_MANAGER_H_
#define _FLURRY_MANAGER_H_


#include <string>


class FlurryManagerImpl;


namespace Triniti2D {


//! @class FlurryManager
//! @brief FlurryManager
class FlurryManager
{
public:
	//! 初始化
	static void Initialize(const std::string& version, const std::string& key);

private:
	//! 实现
	static FlurryManagerImpl* m_impl;
};


} // namespace Triniti2D


#endif // _FLURRY_MANAGER_H_
