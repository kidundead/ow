//
//  UnityEAGLView.h
//  Cube
//
//  Created by shi william on 09-12-16.
//  Copyright 2009 masq. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <OpenGLES/EAGL.h>
#import <OpenGLES/EAGLDrawable.h>
#import <OpenGLES/ES1/gl.h>
#import <OpenGLES/ES1/glext.h>
#import <QuartzCore/QuartzCore.h>

#define CALL_EAGL_FUNCTION(__FUNC__, ...) ({ EAGLError __error = __FUNC__( __VA_ARGS__ ); if(__error != kEAGLErrorSuccess) printf("%s() called from %s returned error %i\n", #__FUNC__, __FUNCTION__, __error); (__error ? 0 : 1); })
#define CHECK_GL_ERROR() ({ GLenum __error = glGetError(); if(__error) printf_console("OpenGLES error 0x%04X in %s\n", __error, __FUNCTION__); (__error ? NO : YES); })
#define EAGL_ERROR(action) ({ printf_console("Failed to %s. Called from %s\n", action, __FUNCTION__);})
// --- OpenGLES --------------------------------------------------------------------
//

struct MyEAGLSurface
{
	GLuint		format;
	GLuint		depthFormat;
	GLuint		framebuffer;
	GLuint		renderbuffer;
	GLuint		depthBuffer;	
	CGSize		size; 
};

typedef EAGLContext*	MyEAGLContext;

@interface EAGLView : UIView {
}

@end

int OpenEAGL_UnityCallback(int* screenWidth, int* screenHeight);

