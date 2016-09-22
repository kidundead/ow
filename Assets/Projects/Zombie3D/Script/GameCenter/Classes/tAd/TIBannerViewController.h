//
//  TIBannerView.h
//  tAd
//
//  Copyright 2011 Triniti, Inc. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <iAd/iAd.h>

enum tADBannerSize {
	tADBannerContentSizeIdentifierPortrait = 0,
	tADBannerContentSizeIdentifierLandscape = 1,
};
/*
enum tADBannerAnimationMode {
	tADBannerViewAnimationModeNone = 0,
	tADBannerViewAnimationModeMoveIn,
	tADBannerViewAnimationModeMoveInOut,
	tADBannerViewAnimationModeEaseIn,
	tADBannerViewAnimationModeEaseInOut,
};
*/

@class TIBannerView;
@class TIPopupView;


@protocol TIBannerViewDelegate;
// --banner
@interface TIBannerView : UIView <UIWebViewDelegate>
{
	UIWebView *bannerWebView;
	UIImageView *bannerDefaultView;
	NSString *appAddress;
@private
	id <TIBannerViewDelegate> _delegate;
}
@property (nonatomic, assign) id <TIBannerViewDelegate> delegate;

- (id)initWithDelegate:(id)delegate;

@end

// --banner protocol
@protocol TIBannerViewDelegate <NSObject>
@optional

- (void)bannerWebViewDidLoad;
- (void)popView;

@end



@protocol TIPopupViewDelegate;
// --popup
@interface TIPopupView : UIView <UIWebViewDelegate>
{
	CGRect webFrame;
	//UIActivityIndicatorView *spinner;
	
@private
	id <TIPopupViewDelegate> _delegate;
}
@property (nonatomic, assign) id <TIPopupViewDelegate> delegate;

- (id)initWithDelegate:(id)delegate;

@end

// --popup protocol
@protocol TIPopupViewDelegate <NSObject>
@optional

- (void)popViewRemoved;

@end





//NS_CLASS_AVAILABLE(NA, 4_0) @interface TIBannerView : UIView
// view controller
@interface TIBannerViewController : UIViewController <ADBannerViewDelegate, TIBannerViewDelegate, TIPopupViewDelegate>
{
	TIBannerView *banner;
	//TIPopupView *popup;
	
	BOOL bAutoMoveOut;
	
	
	// iAd
	//ADBannerView *adView;
	BOOL bannerIsVisible;
}

- (id)initWithProjectName:(NSString *)projectName isAutoDispare:(BOOL)moveOut;
- (id)initWithProjectName:(NSString *)projectName isAutoDispare:(BOOL)moveOut defaultShow:(BOOL)bShow;
- (id)initWithProjectName:(NSString *)projectName isAutoDispare:(BOOL)moveOut defaultShow:(BOOL)bShow gameKind:(NSString *)game;
- (void)showBanner:(BOOL)bShow;

+ (id)defaultController;

@end