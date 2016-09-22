//
//  MKStoreObserver.m
//
//  Created by Mugunth Kumar on 17-Oct-09.
//  Copyright 2009 Mugunth Kumar. All rights reserved.
//  mugunthkumar.com
//

#import "MKStoreObserver.h"
#import "MKStoreManager.h"

@implementation MKStoreObserver

- (void)paymentQueue:(SKPaymentQueue *)queue updatedTransactions:(NSArray *)transactions
{
	bool purchasing=false;
	for (SKPaymentTransaction *transaction in transactions)
	{
		switch (transaction.transactionState)
		{
			case SKPaymentTransactionStatePurchased:
				[[MKStoreManager sharedManager] provideContent: transaction.payment.productIdentifier];
				break;
			case SKPaymentTransactionStateFailed:
				if (transaction.error.code != SKErrorPaymentCancelled)
				{
					[[MKStoreManager sharedManager] failedTransaction: transaction];		
				}
				else if (transaction.error.code == SKErrorPaymentCancelled)
				{
					[[MKStoreManager sharedManager] userCanceled];
				}
				break;
			case SKPaymentTransactionStateRestored:
				[[MKStoreManager sharedManager] provideContent: transaction.payment.productIdentifier];
				break;
			case SKPaymentTransactionStatePurchasing:
				purchasing=true;
			default:
				break;
		}
		if(!purchasing)
			[[SKPaymentQueue defaultQueue] finishTransaction: transaction];
	}
}



@end
