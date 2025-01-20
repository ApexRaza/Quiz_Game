#import <Foundation/Foundation.h>
#import <Social/Social.h>

extern "C" {
    void ShareTextWithURL(const char* message, const char* url) {
        NSString *textToShare = [NSString stringWithUTF8String:message];
        NSURL *urlToShare = [NSURL URLWithString:[NSString stringWithUTF8String:url]];
        
        NSArray *itemsToShare = @[textToShare, urlToShare];
        
        UIActivityViewController *activityVC = [[UIActivityViewController alloc] initWithActivityItems:itemsToShare applicationActivities:nil];
        
        UIViewController *rootViewController = [UIApplication sharedApplication].keyWindow.rootViewController;
        
        // For iPad, we need to present it in a popover
        if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad) {
            activityVC.modalPresentationStyle = UIModalPresentationPopover;
            UIPopoverPresentationController *popover = activityVC.popoverPresentationController;
            popover.sourceView = rootViewController.view;
            popover.sourceRect = CGRectMake(rootViewController.view.frame.size.width/2, 
                                          rootViewController.view.frame.size.height/2, 
                                          0, 
                                          0);
            popover.permittedArrowDirections = 0;
        }
        
        [rootViewController presentViewController:activityVC animated:YES completion:nil];
    }
} 
