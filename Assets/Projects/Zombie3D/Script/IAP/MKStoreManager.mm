//
//  MKStoreManager.m
//
//  Created by Mugunth Kumar on 17-Oct-09.
//  Copyright 2009 Mugunth Kumar. All rights reserved.
//  mugunthkumar.com
//

#import "MKStoreManager.h"

@implementation MKStoreManager

@synthesize purchasableObjects;
@synthesize storeObserver;
@synthesize lastIdentifier;

// all your features should be managed one and only by StoreManager
static NSString *featureAId = @"com.trinitigame.callofminizombies.099cents";
static NSString *featureBId = @"com.trinitigame.callofminizombies.199cents";
 // @"com.trinitigame.touchfish.199cents", @"com.trinitigame.touchfish.099cents", @"com.trinitigame.touchfish.999cents", @"com.trinitigame.touchfish.1999cents", @"com.trinitigame.touchfish.4999cents", @"com.trinitigame.touchfish.9999cents"";
//static NSString *featureBId = @"com.mycompany.myapp.myfeatureB";


BOOL featureAPurchased;
BOOL featureBPurchased;
int _purchaseStatus;

static MKStoreManager* _sharedStoreManager; // self

- (void)dealloc {
	
	[_sharedStoreManager release];
	[storeObserver release];
	[super dealloc];
}

+ (BOOL) featureAPurchased {
	
	return featureAPurchased;
}

+ (BOOL) featureBPurchased {
	
	return featureBPurchased;
}

+ (MKStoreManager*)sharedManager
{
	@synchronized(self) {
		
        if (_sharedStoreManager == nil) {
			
            [[self alloc] init]; // assignment not done here
			
			_sharedStoreManager.purchasableObjects = [[NSMutableArray alloc] init];		
			[MKStoreManager loadPurchases];
			_sharedStoreManager.storeObserver = [[MKStoreObserver alloc] init];
			//[_sharedStoreManager requestProductData];
			[[SKPaymentQueue defaultQueue] addTransactionObserver:_sharedStoreManager.storeObserver];
        }
    }
    return _sharedStoreManager;
}


#pragma mark Singleton Methods

+ (id)allocWithZone:(NSZone *)zone

{	
    @synchronized(self) {
		
        if (_sharedStoreManager == nil) {
			
            _sharedStoreManager = [super allocWithZone:zone];			
            return _sharedStoreManager;  // assignment and return on first allocation
        }
    }
	
    return nil; //on subsequent allocation attempts return nil	
}


- (id)copyWithZone:(NSZone *)zone
{
    return self;	
}

- (id)retain
{	
    return self;	
}

- (unsigned)retainCount
{
    return UINT_MAX;  //denotes an object that cannot be released
}

- (void)release
{
    //do nothing
}

- (id)autorelease
{
    return self;	
}


- (void) requestProductData:(NSString*)productIdentifier
{
	SKProductsRequest *request= [[SKProductsRequest alloc] initWithProductIdentifiers: 
								 [NSSet setWithObjects: productIdentifier, nil]]; // add any other product here
	request.delegate = self;
	[request start];
}


- (void)productsRequest:(SKProductsRequest *)request didReceiveResponse:(SKProductsResponse *)response
{
	NSArray* products = response.products;
	NSLog(@"products count is %d", [products count]);
	
	
	if ([response.invalidProductIdentifiers count] > 0){
        NSLog(@"MKStoreManager-productsRequest empty results: %@", [response.invalidProductIdentifiers objectAtIndex:0]);
    }
    
    
	for(int i=0;i<[products count];i++)
	{
		
		SKProduct *product = [products objectAtIndex:i];		
		if(![_sharedStoreManager.purchasableObjects containsObject:product])
			[_sharedStoreManager.purchasableObjects addObject:product];
		
		NSLog(@"Feature: %@, Cost: %f, ID: %@",[product localizedTitle],
			  [[product price] doubleValue], [product productIdentifier]);		
		if([lastIdentifier isEqualToString:[product productIdentifier]])
		{
			NSLog(@"Add Payment: %@", [product productIdentifier]);
			SKPayment *payment = [SKPayment paymentWithProductIdentifier:lastIdentifier];
			[[SKPaymentQueue defaultQueue] addPayment:payment];	
		}
	}
	
	
	[request autorelease];
}
/*
- (void) buyFeatureA
{
	[self buyFeature:featureAId];
}
*/
- (void) buyFeature:(NSString*) featureId productCount:(NSString *)count
{	
	_purchaseStatus = 0;
	productCount =atoi([count UTF8String]);	
	lastIdentifier = featureId;
	if ([SKPaymentQueue canMakePayments])
	{
		for(int i=0;i<[_sharedStoreManager.purchasableObjects count];i++)
		{
			SKProduct *product = [_sharedStoreManager.purchasableObjects objectAtIndex:i];
			if([lastIdentifier isEqualToString:[product productIdentifier]])
			{
				SKPayment *payment = [SKPayment paymentWithProductIdentifier:featureId];
				//payment.quantity=productCount;
				[[SKPaymentQueue defaultQueue] addPayment:payment];	
				return;
			}
		}
		[_sharedStoreManager requestProductData:featureId];
	}
	else
	{
		[self userCanceled];
		UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"MKStoreKit" message:@"You are not authorized to purchase from AppStore"
													   delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
		[alert show];
		[alert release];
	}	
}
/*
- (void) buyFeatureB
{
	[self buyFeature:featureBId];
}000
*/

- (void) failedTransaction: (SKPaymentTransaction *)transaction
{
	NSString *messageToBeShown = [NSString stringWithFormat:@"Reason: %@, You can try: %@", [transaction.error localizedFailureReason], [transaction.error localizedRecoverySuggestion]];
	UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Unable to complete your purchase" message:messageToBeShown
												   delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
	[alert show];	
	[alert release];
	_purchaseStatus = -1;
}

-(void) provideContent: (NSString*) productIdentifier
{
	/*if([productIdentifier isEqualToString:featureAId])
		featureAPurchased = YES;

	if([productIdentifier isEqualToString:featureBId])
		featureBPurchased = YES;
	*/
	_purchaseStatus = 1;
	//[MKStoreManager updatePurchases];
}
-(void) userCanceled
{
	_purchaseStatus = -2;
}

+(void) loadPurchases 
{
	NSUserDefaults *userDefaults = [NSUserDefaults standardUserDefaults];	
	featureAPurchased = [userDefaults boolForKey:featureAId]; 
	featureBPurchased = [userDefaults boolForKey:featureBId]; 	
}


+(void) updatePurchases
{
	NSUserDefaults *userDefaults = [NSUserDefaults standardUserDefaults];
	[userDefaults setBool:featureAPurchased forKey:featureAId];
	[userDefaults setBool:featureBPurchased forKey:featureBId];
}

-(int) purchaseStatus
{
	return _purchaseStatus;
}
@end
