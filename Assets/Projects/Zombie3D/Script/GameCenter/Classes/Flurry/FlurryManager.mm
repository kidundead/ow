
#include "FlurryManager.h"
#include "FlurryManagerImpl.h"
#include <assert.h>


namespace Triniti2D {


// class FlurryManager
FlurryManagerImpl* FlurryManager::m_impl = NULL;

void FlurryManager::Initialize(const std::string& version, const std::string& key)
{
	if (m_impl == NULL)
	{
		m_impl = new ::FlurryManagerImpl;
		m_impl->Initialize(version, key);
	}
}


} // namespace Triniti2D

