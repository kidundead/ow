
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#import <UIKit/UIKit.h>
#import <OpenGLES/EAGL.h>
#import <OpenGLES/EAGLDrawable.h>
#import <OpenGLES/ES1/gl.h>
#import <OpenGLES/ES1/glext.h>
#import <QuartzCore/QuartzCore.h>


extern "C" void AvataTakePhotoPlugin(const char* save_path, const char* photo_key)
{
	//
	GLubyte* buffer1 = new GLubyte[480 * 320 * 4];
	GLubyte* buffer2 = new GLubyte[480 * 320 * 4];

	//
	glReadPixels(0, 0, 320, 480, GL_RGBA, GL_UNSIGNED_BYTE, buffer1);

	//
	for (int y = 0; y < 480; y++)
	{
		for (int x = 0; x < 320; x++)
		{
			buffer2[((480 - 1 - y) * 320 + x) * 4 + 0] = buffer1[(y * 320 + x) * 4 + 0];
			buffer2[((480 - 1 - y) * 320 + x) * 4 + 1] = buffer1[(y * 320 + x) * 4 + 1];
			buffer2[((480 - 1 - y) * 320 + x) * 4 + 2] = buffer1[(y * 320 + x) * 4 + 2];
			buffer2[((480 - 1 - y) * 320 + x) * 4 + 3] = buffer1[(y * 320 + x) * 4 + 3];
		}
	}

	for (int y = 0; y < 480; y++)
	{
		for (int x = 0; x < 320; x++)
		{
			buffer1[((320 - 1 - x) * 480 + y) * 4 + 0] = buffer2[(y * 320 + x) * 4 + 0];
			buffer1[((320 - 1 - x) * 480 + y) * 4 + 1] = buffer2[(y * 320 + x) * 4 + 1];
			buffer1[((320 - 1 - x) * 480 + y) * 4 + 2] = buffer2[(y * 320 + x) * 4 + 2];
			buffer1[((320 - 1 - x) * 480 + y) * 4 + 3] = buffer2[(y * 320 + x) * 4 + 3];
		}
	}

	// 创建图像
	CGDataProviderRef providerRef = CGDataProviderCreateWithData(NULL, buffer1, 480 * 320 * 4, NULL);
	CGColorSpaceRef colorSpaceRef = CGColorSpaceCreateDeviceRGB();
	CGImageRef imageRef = CGImageCreate(480, 320, 8, 32, 480 * 4, colorSpaceRef, kCGBitmapByteOrderDefault, providerRef, NULL, NO, kCGRenderingIntentDefault);
	UIImage* image = [UIImage imageWithCGImage:imageRef];

	// 保存到相册
	UIImageWriteToSavedPhotosAlbum(image, nil, @selector(image:didFinishSavingWithError:contextInfo:), nil);

	/*
	// 保存到应用数据目录
	char filename[1024];
	strcpy(filename, save_path);
	strcat(filename, "/");
	strcat(filename, photo_key);
	strcat(filename, "_photo.png");
	NSString* photoPath = [NSString stringWithCString:filename encoding:NSUTF8StringEncoding];
	[UIImagePNGRepresentation(image) writeToFile:photoPath atomically:YES];

	// 生成缩略图
	CGImageRef image2Ref = CGImageCreateWithImageInRect([image CGImage], CGRectMake(200, 110, 78, 115));
	UIImage *thumbnailImage = [UIImage imageWithCGImage:image2Ref];

	// 保存缩略图
	strcpy(filename, save_path);
	strcat(filename, "/");
	strcat(filename, photo_key);
	strcat(filename, "_thumbnail.png");
	NSString* thumbnailPath = [NSString stringWithCString:filename encoding:NSUTF8StringEncoding];
	[UIImagePNGRepresentation(thumbnailImage) writeToFile:thumbnailPath atomically:YES];
	*/

	// 释放对象
	//CGImageRelease(image2Ref);
	CGImageRelease(imageRef);
	CGColorSpaceRelease(colorSpaceRef);
	CGDataProviderRelease(providerRef);

	// 释放缓冲
//	delete [] buffer1;
	delete [] buffer2;
}

