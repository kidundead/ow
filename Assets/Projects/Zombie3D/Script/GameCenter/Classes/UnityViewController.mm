//
//  WebViewController.mm
//
//  Created by Jiang Xiao Ying on 09-11-13.
//  Copyright 2009 . All rights reserved.
//

#import "UnityViewController.h"
#import "UnityEAGLView.h"

//UnityViewController *gUnityViewController;
@implementation UnityViewController


// Implement loadView to create a view hierarchy programmatically, without using a nib.
- (void)loadView {
	
	EAGLView* view = [[EAGLView alloc] initWithFrame:[[UIScreen mainScreen] bounds]];
	self.view = view;

		
}
-(void)viewDidLoad{
	//private API,should use other method
	
}

- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation {
	return NO;//(interfaceOrientation==UIInterfaceOrientationLandscapeRight);
}



- (void)dealloc {
    [super dealloc];
}


@end
