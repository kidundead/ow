
#include "FlurryManagerImpl.h"
#import "FlurryAPI.h"


// class FlurryManagerImpl
void FlurryManagerImpl::Initialize(const std::string& version, const std::string& key)
{
	NSString* v = [NSString stringWithCString:version.c_str() encoding:NSUTF8StringEncoding];
	NSString* k = [NSString stringWithCString:key.c_str() encoding:NSUTF8StringEncoding];

	[FlurryAPI setAppVersion:v];
	[FlurryAPI startSession:k];
}

