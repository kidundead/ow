//
//  MessageBoxDelegate.h
//  MessageBox
//
//  Created by chengpu on 6/21/10.
//  Copyright __MyCompanyName__ 2010. All rights reserved.
//

#import <UIKit/UIKit.h>


int OpenMessageBox(const char* title, const char* message, int button_number, const char* button_name[]);


@interface MessageBoxDelegate : NSObject<UIAlertViewDelegate>
{
	int _buttonNumber;
}

@property (nonatomic) int _buttonNumber;

- (void)alertView:(UIAlertView*)alertView clickedButtonAtIndex:(NSInteger)buttonIndex;
- (void)didPresentAlertView:(UIAlertView*)alertView;

@end


