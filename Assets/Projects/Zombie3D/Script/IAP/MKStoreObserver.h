//
//  MKStoreObserver.m
//
//  Created by Mugunth Kumar on 17-Oct-09.
//  Copyright 2009 Mugunth Kumar. All rights reserved.
//  mugunthkumar.com
//

#import <Foundation/Foundation.h>
#import <StoreKit/StoreKit.h>

@interface MKStoreObserver : NSObject<SKPaymentTransactionObserver> {

	
}
	
- (void)paymentQueue:(SKPaymentQueue *)queue updatedTransactions:(NSArray *)transactions;


@end
