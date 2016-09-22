#include "TAdPlugin.h"
#import "TIBannerViewController.h"
#import <UIKit/UIKit.h>
#include "FlurryManager.h"

extern "C" UIViewController * g_ViewController;

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
        UIWindow *window = [[UIApplication sharedApplication] keyWindow ];
		[[[UIApplication sharedApplication] keyWindow ] addSubview:controller.view];
        //[g_ViewController.view addSubview:controller.view];
        //[g_ViewController presentModalViewController:controller animated:NO];
        //[g_ViewController.view addSubview:controller.view];
		[controller release];
	}
	else 
	{
		if (bVisible) 
		{
            
        }
        
        [[TIBannerViewController defaultController] showBanner:bVisible?YES:NO];
	}
}


void RotateTadView()
{
    UIDeviceOrientation oritentation = [[UIDevice currentDevice] orientation];
    [ [TIBannerViewController defaultController] rotateForUnity:oritentation];
}


void RotateTadForUnity(UIInterfaceOrientation curOrientation)
{
    //[[TIBannerViewController defaultController] rotateForUnity];
}

void Initialize(const char* version, const char* key)
{
    Triniti2D::FlurryManager::Initialize(std::string(version), std::string(key));
}

