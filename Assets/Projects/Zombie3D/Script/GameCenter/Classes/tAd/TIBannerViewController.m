//
//  TIBannerView.m
//  iAdTestProject
//
//  Created by zhudongjie on 1/11/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <QuartzCore/QuartzCore.h>
#import "TIBannerViewController.h"
#import "DownloadHelper.h"

//# ifdef TORQUE_OS_IPHONE
//#include "platformiPhone.h"
//extern iPhonePlatState platState;

int curStatus = tADBannerContentSizeIdentifierPortrait;
BOOL defaultShow = YES;
CGPoint bannerCenter;
CGRect bannerBounds;
CGPoint popupCenter;
CGRect popupBounds;
char currentProjectNameUTF8[1024];

TIBannerViewController *_defaultViewController;

#pragma mark -
@implementation TIBannerView
@synthesize delegate = _delegate;

- (id)initWithDelegate:(id)delegate
{
	self.delegate = delegate;
	[self.delegate retain];
	
	CGRect frame = bannerBounds;
	self = [super initWithFrame:frame];
	if (self) {
		self.userInteractionEnabled = YES;
		
		self.backgroundColor = [UIColor colorWithWhite:0 alpha:0];
		
		NSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];
		
		bannerWebView = [[UIWebView alloc] initWithFrame:frame];
		//bannerWebView.backgroundColor = [UIColor colorWithWhite:0 alpha:0.5f];
		//banner.autoresizesSubviews = YES;
		bannerWebView.userInteractionEnabled = NO;
		
		bannerWebView.delegate = self;
		
		[self addSubview:bannerWebView];
		
		NSString *urlAddress = [NSString stringWithFormat:@"http://www.trinitigame.com/tad/%@/smallbanner.html", [NSString stringWithUTF8String:currentProjectNameUTF8]];
		
		
		
		
		NSLog(@"==>%@", urlAddress);
//		NSURL *url = [NSURL URLWithString:urlAddress];
		
		[DownloadHelper sharedInstance].delegate = self;
		[DownloadHelper download:urlAddress];
		
	
		
//		// default.png
//		NSArray *paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
//		NSString *documentsPath = [paths objectAtIndex:0];
//		NSString *fullPath = [documentsPath stringByAppendingString:@"/tAdDefaultBannerImageForSlotUse.dat"];
//		if ([[NSFileManager defaultManager] fileExistsAtPath:fullPath]) {
//			NSData *dataFromFile = [NSData dataWithContentsOfFile:fullPath];
//			UIImage *bannerDefaultImg = [UIImage imageWithData:dataFromFile];
//			bannerDefaultView = [[UIImageView alloc] initWithImage:bannerDefaultImg];
//			[self addSubview:bannerDefaultView];
//		}
//		else {
//			bannerWebView.hidden = YES;
//		}
//		
//		// download the newest default.png
//		NSString *urlAdressDefaultBannerImg = [NSString stringWithFormat:@"http://www.trinitigame.com/tad/%@/smallbanner.jpg", [NSString stringWithUTF8String:currentProjectNameUTF8]];
//		NSURL *urlForDataWrite = [NSURL URLWithString:urlAdressDefaultBannerImg];
//		NSData *data = [NSData dataWithContentsOfURL:urlForDataWrite];
//		[data writeToFile:fullPath atomically:YES];
		
//		// loading animation
//		UIActivityIndicatorView *spinner = [[UIActivityIndicatorView alloc] initWithActivityIndicatorStyle:UIActivityIndicatorViewStyleGray];
//		[spinner setCenter:self.center];
//		spinner.tag = 2;
//		[self addSubview:spinner];
//		[spinner startAnimating];
//		[spinner release];
		
		[pool release];
	}
	return self;
}
- (void) didReceiveData: (NSData *) smallBannerData
{
	NSString *smallBannerString = [[NSString alloc] initWithData:smallBannerData encoding:NSASCIIStringEncoding];
	NSRange appStoreAddressRange = [smallBannerString rangeOfString:@"http://"]; // any address supported
	NSLog(@"++> %d, %d", appStoreAddressRange.location, appStoreAddressRange.length);
	// if there's no appStore address in smallbanner.html
	if (appStoreAddressRange.length <= 0) {
		appAddress = [NSString stringWithFormat:@"http://www.trinitigame.com/tad/%@/redirect.html", [NSString stringWithUTF8String:currentProjectNameUTF8]];
	}
	else {
		NSRange searchRange = NSMakeRange(appStoreAddressRange.location, smallBannerString.length - 1 - appStoreAddressRange.location);
		searchRange = [smallBannerString rangeOfString:@"\">" options:1 range:searchRange];
		if (searchRange.length <= 0) {
			appAddress = [NSString stringWithFormat:@"http://www.trinitigame.com/tad/%@/redirect.html", [NSString stringWithUTF8String:currentProjectNameUTF8]];
		}
		else {
			searchRange.length = searchRange.location - appStoreAddressRange.location;
			searchRange.location = appStoreAddressRange.location;
			appAddress = [smallBannerString substringWithRange:searchRange];
		}
	}
	[appAddress retain];
	
	//		NSURLRequest *requestObj = [NSURLRequest requestWithURL:url];
	//		[bannerWebView loadRequest:requestObj];
	//		bannerWebView.delegate = self;
	
	[bannerWebView loadData:smallBannerData
				   MIMEType:@"text/html"
		   textEncodingName:@"UTF-8"
					baseURL:[NSURL URLWithString:@"http://www.trinitigame.com"]];
}
- (void) dataDownloadFailed: (NSString *) reason
{
	NSLog(@"%@",reason);
	self.hidden = YES;
}
- (void)webViewDidFinishLoad:(UIWebView *)webView
{
//	[NSTimer scheduledTimerWithTimeInterval:0.5f target:self selector:@selector(removeDefaultImageAfterWebViewLoaded) userInfo:nil repeats:NO];
	
	
	[self.delegate bannerWebViewDidLoad];
}
//- (void)removeDefaultImageAfterWebViewLoaded
//{
//	[bannerDefaultView removeFromSuperview];
//}

- (void)touchesBegan:(NSSet *)touches withEvent:(UIEvent *)event {
	NSLog(@"banner view : touches began...");
//	[self.delegate popView];
	
	// redirect
	//NSString *urlAddress = [NSString stringWithFormat:@"http://www.trinitigame.com/tad/%@/redirect.html", [NSString stringWithUTF8String:currentProjectNameUTF8]];
	NSLog(@"===> %@", appAddress);
	if (appAddress) {
		NSURL *url = [NSURL URLWithString:appAddress];
		[[UIApplication sharedApplication] openURL:url];
	}
	
//	// appStore
//	NSString *urlAddressApp = [NSString stringWithFormat:@"http://itunes.apple.com/us/app/gamebox-2/id371254525?mt=8"];
//	NSURL *urlApp = [NSURL URLWithString:urlAddressApp];
//	[[UIApplication sharedApplication] openURL:urlApp];
}

- (void)dealloc
{
	[bannerWebView release];
	[self.delegate release];
	self.delegate = nil;
    [super dealloc];
}

@end



#pragma mark -
@implementation TIPopupView
@synthesize delegate = _delegate;

- (id)initWithDelegate:(id)delegate
{
	self.delegate = delegate;
	[self.delegate retain];
	
	NSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];
	
	CGRect frame = popupBounds;
	self = [super initWithFrame:frame];
	if (self) {
		self.backgroundColor = [UIColor colorWithWhite:0 alpha:0.8f];
		self.userInteractionEnabled = YES;
		
//		[[NSNotificationCenter defaultCenter] addObserver: self
//												 selector: @selector(enteredBackground)
//													 name: @"didEnterBackground"
//												   object: nil];
		
		webFrame = CGRectMake(60, 40, 360, 240);
		UIWebView *popupWebView = [[UIWebView alloc] initWithFrame:webFrame];
		//[popupWebView setBackgroundColor:[UIColor colorWithWhite:0 alpha:0.4f]];
		//popupWebView.userInteractionEnabled = YES;
		popupWebView.userInteractionEnabled = NO;
		NSString *urlAddress = [NSString stringWithFormat:@"http://www.trinitigame.com/tad/%@/bigbanner.html", [NSString stringWithUTF8String:currentProjectNameUTF8]];
		//NSString *urlAddress=@"http://www.163.com";//@"http://www.sina.com.cn";
		NSLog(@"==>%@", urlAddress);
		NSURL *url = [NSURL URLWithString:urlAddress];
		NSURLRequest *requestObj = [NSURLRequest requestWithURL:url];
		[popupWebView loadRequest:requestObj];
		popupWebView.delegate = self;
		[self addSubview:popupWebView];
		[popupWebView release];
		
		// loading animation
		UIActivityIndicatorView *spinner = [[UIActivityIndicatorView alloc] initWithActivityIndicatorStyle:UIActivityIndicatorViewStyleGray];
		[spinner setCenter:self.center];
		spinner.tag = 2;
		[self addSubview:spinner];
		[spinner startAnimating];
		[spinner release];
	}
	
	[pool release];
	
	return self;
}

- (void)webViewDidFinishLoad:(UIWebView *)webView
{
	UIActivityIndicatorView *spinner = (UIActivityIndicatorView *)[self viewWithTag:2];
	[spinner stopAnimating];
	[spinner removeFromSuperview];
}

- (void)exitWebView
{
	self.superview.center = bannerCenter;
	self.superview.bounds = bannerBounds; // reset superview's frame
	[self.delegate popViewRemoved];
	[self.delegate release];
	self.delegate = nil;
	[self removeFromSuperview];
}
- (void)touchesEnded:(NSSet *)touches withEvent:(UIEvent *)event
{
	UITouch *touch = [touches anyObject];
	CGPoint position = [touch locationInView:self];
	if (position.x > webFrame.origin.x && position.x < webFrame.origin.x + webFrame.size.width
		&& position.y > webFrame.origin.y && position.y < webFrame.origin.y + webFrame.size.height) {
		NSString *urlAddress = [NSString stringWithFormat:@"http://www.trinitigame.com/tad/%@/redirect.html", [NSString stringWithUTF8String:currentProjectNameUTF8]];
		//NSLog(@"*****%@", urlAddress);
		NSURL *url = [NSURL URLWithString:urlAddress];
		[[UIApplication sharedApplication] openURL:url];
	}
	[self exitWebView]; // self exit wherever touches the screen
}

- (void)dealloc
{
	[super dealloc];
}

@end



#pragma mark -
@implementation TIBannerViewController

+ (id)defaultController
{
	if (_defaultViewController == nil) {
		return nil;
	}
	else {
		return _defaultViewController;
	}
}

- (id)initWithProjectName:(NSString *)projectName isAutoDispare:(BOOL)moveOut// application:(UIApplication *)application
{
	if (_defaultViewController) {
		return _defaultViewController;
	}
	
	const char *pNameUTF8 = [projectName UTF8String];
	memcpy(currentProjectNameUTF8, pNameUTF8, 1024);
	
	bAutoMoveOut = moveOut;
	
	//[[UIApplication sharedApplication] setStatusBarOrientation:UIInterfaceOrientationLandscapeRight];
	//application.statusBarOrientation = UIInterfaceOrientationLandscapeRight;
	//[application setStatusBarOrientation:UIInterfaceOrientationPortrait];
	
	self = [super init];
	if (self) {
		curStatus = tADBannerContentSizeIdentifierLandscape;
		
		bannerCenter = CGPointMake(16,240); // 240 16
		bannerBounds = CGRectMake(0, 0, 480 ,32);
		popupCenter = CGPointMake(160, 240); // 240 160
		popupBounds = CGRectMake(0, 0, 480, 320);
		
		self.view.backgroundColor = [UIColor colorWithWhite:0 alpha:0];
		self.view.hidden = YES;
        
        CGAffineTransform transform = self.view.transform;
        transform = CGAffineTransformRotate(transform, M_PI/2.0);
        self.view.transform = transform;

		[NSTimer scheduledTimerWithTimeInterval:0.01f target:self selector:@selector(resizeCurrentViewAfterRotation) userInfo:nil repeats:NO];
	}
	else {
		self = nil;
	}
	NSAssert(self != nil, @"~tAd Initialize failed!!~");

	_defaultViewController = self;
	return self;
}
- (id)initWithProjectName:(NSString *)projectName isAutoDispare:(BOOL)moveOut defaultShow:(BOOL)bShow
{
	defaultShow = bShow;
	return [self initWithProjectName:projectName isAutoDispare:moveOut];
}
- (id)initWithProjectName:(NSString *)projectName isAutoDispare:(BOOL)moveOut defaultShow:(BOOL)bShow gameKind:(NSString *)game
{
	defaultShow = bShow;
	id tempTon = [self initWithProjectName:projectName isAutoDispare:moveOut];
	
	// if not TGB, then resize it!!
	if ([game isEqualToString:@"gamebox"]) {
		bannerCenter = CGPointMake(240, 304);
		bannerBounds = CGRectMake(0, 0, 480, 32);
	}
	
	return tempTon;
}

- (void)viewDidLoad
{
	if (defaultShow == YES) {
		self.view.hidden = NO;
	}
	// banner view
	banner = [[TIBannerView alloc] initWithDelegate:self];
}

- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation
{
	return (interfaceOrientation == UIInterfaceOrientationLandscapeRight);
}

- (void)resizeCurrentViewAfterRotation
{
	self.view.center = bannerCenter;
	self.view.bounds = bannerBounds;
	
//	NSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];
	
	//NSLog(@"uniqueIdentifier++> %@", [[UIDevice currentDevice] uniqueIdentifier]);
	//NSLog(@"systemVersion => %@", [[UIDevice currentDevice] systemVersion]);
	float sysVersion = 0;//[[UIDevice currentDevice] systemVersion] floatValue];
	if (sysVersion > 4.19999f) {
		// iAd
//		ADBannerView *adView = [[ADBannerView alloc] initWithFrame:bannerBounds];
//		adView.userInteractionEnabled = YES;
//		adView.requiredContentSizeIdentifiers = [NSSet setWithObject:ADBannerContentSizeIdentifierLandscape];
//		adView.currentContentSizeIdentifier = ADBannerContentSizeIdentifierLandscape;
//		[self.view addSubview:adView];
//		[adView release];
//		adView.delegate = self;
//		bannerIsVisible = NO;
	}
	else {
		[self.view addSubview:banner];
	}

	

	
	
//	CATransition *animation = [CATransition animation];
//	[animation setDuration:0.5f];
//	[animation setType:kCATransitionMoveIn];
//	[animation setSubtype:kCATransitionFromTop];
//	[animation setTimingFunction:[CAMediaTimingFunction functionWithName:kCAMediaTimingFunctionEaseInEaseOut]];
//	[[banner layer] removeAllAnimations];
//	[[banner layer] addAnimation:animation forKey:nil];
	
//	[pool release];
}

- (void)bannerWebViewDidLoad
{
	if (defaultShow == YES) {
		self.view.hidden = NO;
	}
	
	if (bAutoMoveOut) {
		[NSTimer scheduledTimerWithTimeInterval:2 target:self selector:@selector(removeBannerView) userInfo:nil repeats:NO];
	}
}
//- (void)animationDidStop:(CAAnimation*)_animation finished:(BOOL)_finished
//{
//	if (banner.hidden) {
//		[[banner layer] removeAllAnimations];
//	}
//}
- (void)removeBannerView
{
//	if ([[self.view subviews] count] >= 2) {
//		return;
//	}
	
//	NSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];
//	
//	CATransition *animation = [CATransition animation];
//	[animation setDuration:0.5f];
//	[animation setType:kCATransitionReveal];
//	[animation setSubtype:kCATransitionFromBottom];
//	//[animation setFillMode:kCAFillModeBoth];
//	[animation setTimingFunction:[CAMediaTimingFunction functionWithName:kCAMediaTimingFunctionEaseInEaseOut]];
//	animation.delegate = self;
//	[[banner layer] removeAllAnimations];
//	[[banner layer] addAnimation:animation forKey:nil];
//	
//	[pool release];
	
	defaultShow = NO;
	self.view.hidden = !defaultShow;
//	banner.hidden = !defaultShow;
}

- (void)popView
{
	NSLog(@"~pop up view~");
	
	banner.hidden = YES;
	
	self.view.center = popupCenter;
	self.view.bounds = popupBounds;
	
	NSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];
	
	TIPopupView *popup = [[TIPopupView alloc] initWithDelegate:self];
	[self.view addSubview:popup];
	[popup release];
	
	//NSLog(@"===subviews=====> %d", [[self.view subviews] count]);//2
	
	[pool release];
}
- (void)popViewRemoved
{
	if (bAutoMoveOut) {
		[NSTimer scheduledTimerWithTimeInterval:2 target:self selector:@selector(removeBannerView) userInfo:nil repeats:NO];
	}
	
	self.view.center = bannerCenter;
	self.view.bounds = bannerBounds;
	
	banner.hidden = NO;
}


- (void)showBanner:(BOOL)bShow
{
	defaultShow = bShow;
	self.view.hidden = !defaultShow;
//	banner.hidden = !defaultShow;
	
	if (bShow) {
		if (bAutoMoveOut) {
			[NSTimer scheduledTimerWithTimeInterval:2 target:self selector:@selector(removeBannerView) userInfo:nil repeats:NO];
		}
	}
	
//	if (bShow) {
//		self.view.hidden = NO;
//	}
//	else {
//		self.view.hidden = YES;
//	}
}



#pragma mark -
#pragma mark iAd Delegate banner view loaded or fail to load
- (void)bannerViewDidLoadAd:(ADBannerView *)iAdBanner
{
	NSLog(@"~banner view load successfully~");
	
	if (!bannerIsVisible) {
		bannerIsVisible = YES;
		
		//[UIView beginAnimations:@"animateAdBannerOn" context:NULL];
		
		iAdBanner.hidden = NO;
		if (defaultShow == YES) {
			self.view.hidden = NO;
		}
		
		//[UIView commitAnimations];
		
		// auto hide
		if (bAutoMoveOut) {
			[NSTimer scheduledTimerWithTimeInterval:2 target:self selector:@selector(removeBannerView) userInfo:nil repeats:NO];
		}
	}
}

- (void)bannerView:(ADBannerView *)iAdBanner didFailToReceiveAdWithError:(NSError *)error
{
	NSLog(@"~banner view load failed~");
	
	if (bannerIsVisible) {
		bannerIsVisible = NO;
		
		//[UIView beginAnimations:@"animateAdBannerOff" context:NULL];
		
		iAdBanner.hidden = YES;
		
		//[UIView commitAnimations];
	}
	
	
	[self.view addSubview:banner];
	
	iAdBanner.delegate =nil;
	[iAdBanner removeFromSuperview];
}



- (void)dealloc
{
	[banner release];
	//[popup release];
    [super dealloc];
}

@end

