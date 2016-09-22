#import <UIKit/UIKit.h>

#include "RegisterClasses.h"
#include "RegisterMonoModules.h"

int main(int argc, char *argv[])
{
	RegisterMonoModules();
	NSLog(@"-> registered mono modules\n");
	
	NSAutoreleasePool*		pool = [NSAutoreleasePool new];
	
	UIApplicationMain(argc, argv, nil, @"AppController");
	
	[pool release];
	
	return 0;
}
