#include "TAdPlugin.h"
#import "TIBannerViewController.h"
#import <UIKit/UIKit.h>
#include "FlurryManager.h"


void ShowTadView(bool bVisible)
{
	if ([TIBannerViewController defaultController] == nil) 
	{
		if (!bVisible) 
		{
			return;
		}
		TIBannerViewController *controller = [[TIBannerViewController alloc] initWithProjectName:@"callofminizombies" isAutoDispare:NO];
        [[UIApplication sharedApplication] setStatusBarOrientation:UIInterfaceOrientationLandscapeRight ];
		[[[UIApplication sharedApplication] keyWindow] addSubview:controller.view];
		[controller release];
	}
	else 
        
	{
		[[TIBannerViewController defaultController] showBanner:bVisible?YES:NO];
	}
}

void Initialize(const char* version, const char* key)
{
    Triniti2D::FlurryManager::Initialize(std::string(version), std::string(key));
}

