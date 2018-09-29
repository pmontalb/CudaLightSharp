using System;

namespace CudaLightSharp.Exceptions
{
    [Serializable]
    internal class CudaGenericKernelException : Exception
    {
        public CudaGenericKernelException(string kernelName, int errorCode = -1)
            : base(kernelName + " returned " + errorCode)
        {
        }
    };

    #region Cuda Exception mapping

    [Serializable]
    internal class CudaErrorMissingConfigurationException : Exception
    {
        public CudaErrorMissingConfigurationException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * The API call failed because it was unable to allocate enough memory to
    * perform the requested operation.
	*/
    [Serializable]
	internal class CudaErrorMemoryAllocationException : Exception
    {
        public CudaErrorMemoryAllocationException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * The API call failed because the CUDA driver and runtime could not be
    * initialized.
	*/
    [Serializable]
	internal class CudaErrorInitializationErrorException : Exception
    {
        public CudaErrorInitializationErrorException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * An exception occurred on the device while executing a kernel. Common
    * causes include dereferencing an invalid device pointer and accessing
    * out of bounds shared memory. The device cannot be used until
    * ::cudaThreadExit() is called. All existing device memory allocations
    * are invalid and must be reconstructed if the program is to continue
    * using CUDA.
	*/
    [Serializable]
	internal class CudaErrorLaunchFailureException : Exception
    {
        public CudaErrorLaunchFailureException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicated that a previous kernel launch failed. This was previously
    * used for device emulation of kernel launches.
    * \deprecated
    * This error return is deprecated as of CUDA 3.1. Device emulation mode was
    * removed with the CUDA 3.1 release.
	*/
    [Serializable]
	internal class CudaErrorPriorLaunchFailureException : Exception
    {
        public CudaErrorPriorLaunchFailureException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates that the device kernel took too long to execute. This can
    * only occur if timeouts are enabled - see the device property
    * \ref ::cudaDeviceProp::kernelExecTimeoutEnabled kernelExecTimeoutEnabled
    * for more information.
    * This leaves the process in an inconsistent state and any further CUDA work
    * will return the same error. To continue using CUDA, the process must be terminated
    * and relaunched.
	*/
    [Serializable]
	internal class CudaErrorLaunchTimeoutException : Exception
    {
        public CudaErrorLaunchTimeoutException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates that a launch did not occur because it did not have
    * appropriate resources. Although this error is similar to
    * ::cudaErrorInvalidConfiguration, this error usually indicates that the
    * user has attempted to pass too many arguments to the device kernel, or the
    * kernel launch specifies too many threads for the kernel's register count.
	*/
    [Serializable]
	internal class CudaErrorLaunchOutOfResourcesException : Exception
    {
        public CudaErrorLaunchOutOfResourcesException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * The requested device function does not exist or is not compiled for the
    * proper device architecture.
	*/
    [Serializable]
	internal class CudaErrorInvalidDeviceFunctionException : Exception
    {
        public CudaErrorInvalidDeviceFunctionException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates that a kernel launch is requesting resources that can
    * never be satisfied by the current device. Requesting more shared memory
    * per block than the device supports will trigger this error, as will
    * requesting too many threads or blocks. See ::cudaDeviceProp for more
    * device limitations.
	*/
    [Serializable]
	internal class CudaErrorInvalidConfigurationException : Exception
    {
        public CudaErrorInvalidConfigurationException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates that the device ordinal supplied by the user does not
    * correspond to a valid CUDA device.
	*/
    [Serializable]
	internal class CudaErrorInvalidDeviceException : Exception
    {
        public CudaErrorInvalidDeviceException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates that one or more of the parameters passed to the API call
    * is not within an acceptable range of values.
	*/
    [Serializable]
	internal class CudaErrorInvalidValueException : Exception
    {
        public CudaErrorInvalidValueException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates that one or more of the pitch-related parameters passed
    * to the API call is not within the acceptable range for pitch.
	*/
    [Serializable]
	internal class CudaErrorInvalidPitchValueException : Exception
    {
        public CudaErrorInvalidPitchValueException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates that the symbol name/identifier passed to the API call
    * is not a valid name or identifier.
	*/
    [Serializable]
	internal class CudaErrorInvalidSymbolException : Exception
    {
        public CudaErrorInvalidSymbolException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates that the buffer object could not be mapped.
	*/
    [Serializable]
	internal class CudaErrorMapBufferObjectFailedException : Exception
    {
        public CudaErrorMapBufferObjectFailedException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates that the buffer object could not be unmapped.
	*/
    [Serializable]
	internal class CudaErrorUnmapBufferObjectFailedException : Exception
    {
        public CudaErrorUnmapBufferObjectFailedException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates that at least one host pointer passed to the API call is
    * not a valid host pointer.
	*/
    [Serializable]
	internal class CudaErrorInvalidHostPointerException : Exception
    {
        public CudaErrorInvalidHostPointerException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates that at least one device pointer passed to the API call is
    * not a valid device pointer.
	*/
    [Serializable]
	internal class CudaErrorInvalidDevicePointerException : Exception
    {
        public CudaErrorInvalidDevicePointerException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates that the texture passed to the API call is not a valid
    * texture.
	*/
    [Serializable]
	internal class CudaErrorInvalidTextureException : Exception
    {
        public CudaErrorInvalidTextureException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates that the texture binding is not valid. This occurs if you
    * call ::cudaGetTextureAlignmentOffset() with an unbound texture.
	*/
    [Serializable]
	internal class CudaErrorInvalidTextureBindingException : Exception
    {
        public CudaErrorInvalidTextureBindingException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates that the channel descriptor passed to the API call is not
    * valid. This occurs if the format is not one of the formats specified by
    * ::cudaChannelFormatKind, or if one of the dimensions is invalid.
	*/
    [Serializable]
	internal class CudaErrorInvalidChannelDescriptorException : Exception
    {
        public CudaErrorInvalidChannelDescriptorException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates that the direction of the memcpy passed to the API call is
    * not one of the types specified by ::cudaMemcpyKind.
	*/
    [Serializable]
	internal class CudaErrorInvalidMemcpyDirectionException : Exception
    {
        public CudaErrorInvalidMemcpyDirectionException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicated that the user has taken the address of a constant variable,
    * which was forbidden up until the CUDA 3.1 release.
    * \deprecated
    * This error return is deprecated as of CUDA 3.1. Variables in constant
    * memory may now have their address taken by the runtime via
    * ::cudaGetSymbolAddress().
	*/
    [Serializable]
	internal class CudaErrorAddressOfConstantException : Exception
    {
        public CudaErrorAddressOfConstantException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicated that a texture fetch was not able to be performed.
    * This was previously used for device emulation of texture operations.
    * \deprecated
    * This error return is deprecated as of CUDA 3.1. Device emulation mode was
    * removed with the CUDA 3.1 release.
	*/
    [Serializable]
	internal class CudaErrorTextureFetchFailedException : Exception
    {
        public CudaErrorTextureFetchFailedException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicated that a texture was not bound for access.
    * This was previously used for device emulation of texture operations.
    * \deprecated
    * This error return is deprecated as of CUDA 3.1. Device emulation mode was
    * removed with the CUDA 3.1 release.
	*/
    [Serializable]
	internal class CudaErrorTextureNotBoundException : Exception
    {
        public CudaErrorTextureNotBoundException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicated that a synchronization operation had failed.
    * This was previously used for some device emulation functions.
    * \deprecated
    * This error return is deprecated as of CUDA 3.1. Device emulation mode was
    * removed with the CUDA 3.1 release.
	*/
    [Serializable]
	internal class CudaErrorSynchronizationErrorException : Exception
    {
        public CudaErrorSynchronizationErrorException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates that a non-float texture was being accessed with linear
    * filtering. This is not supported by CUDA.
	*/
    [Serializable]
	internal class CudaErrorInvalidFilterSettingException : Exception
    {
        public CudaErrorInvalidFilterSettingException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates that an attempt was made to read a non-float texture as a
    * normalized float. This is not supported by CUDA.
	*/
    [Serializable]
	internal class CudaErrorInvalidNormSettingException : Exception
    {
        public CudaErrorInvalidNormSettingException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * Mixing of device and device emulation code was not allowed.
    * \deprecated
    * This error return is deprecated as of CUDA 3.1. Device emulation mode was
    * removed with the CUDA 3.1 release.
	*/
    [Serializable]
	internal class CudaErrorMixedDeviceExecutionException : Exception
    {
        public CudaErrorMixedDeviceExecutionException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates that a CUDA Runtime API call cannot be executed because
    * it is being called during process shut down, at a point in time after
    * CUDA driver has been unloaded.
	*/
    [Serializable]
	internal class CudaErrorCudartUnloadingException : Exception
    {
        public CudaErrorCudartUnloadingException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates that an unknown internal error has occurred.
	*/
    [Serializable]
	internal class CudaErrorUnknownException : Exception
    {
        public CudaErrorUnknownException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates that the API call is not yet implemented. Production
    * releases of CUDA will never return this error.
    * \deprecated
    * This error return is deprecated as of CUDA 4.1.
	*/
    [Serializable]
	internal class CudaErrorNotYetImplementedException : Exception
    {
        public CudaErrorNotYetImplementedException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicated that an emulated device pointer exceeded the 32-bit address
    * range.
    * \deprecated
    * This error return is deprecated as of CUDA 3.1. Device emulation mode was
    * removed with the CUDA 3.1 release.
	*/
    [Serializable]
	internal class CudaErrorMemoryValueTooLargeException : Exception
    {
        public CudaErrorMemoryValueTooLargeException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates that a resource handle passed to the API call was not
    * valid. Resource handles are opaque types like ::cudaStream_t and
    * ::cudaEvent_t.
	*/
    [Serializable]
	internal class CudaErrorInvalidResourceHandleException : Exception
    {
        public CudaErrorInvalidResourceHandleException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates that asynchronous operations issued previously have not
    * completed yet. This result is not actually an error, but must be indicated
    * differently than ::cudaSuccess (which indicates completion). Calls that
    * may return this value include ::cudaEventQuery() and ::cudaStreamQuery().
	*/
    [Serializable]
	internal class CudaErrorNotReadyException : Exception
    {
        public CudaErrorNotReadyException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates that the installed NVIDIA CUDA driver is older than the
    * CUDA runtime library. This is not a supported configuration. Users should
    * install an updated NVIDIA display driver to allow the application to run.
	*/
    [Serializable]
	internal class CudaErrorInsufficientDriverException : Exception
    {
        public CudaErrorInsufficientDriverException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates that the user has called ::cudaSetValidDevices(),
    * ::cudaSetDeviceFlags(), ::cudaD3D9SetDirect3DDevice(),
    * ::cudaD3D10SetDirect3DDevice, ::cudaD3D11SetDirect3DDevice(), or
    * ::cudaVDPAUSetVDPAUDevice() after initializing the CUDA runtime by
    * calling non-device management operations (allocating memory and
    * launching kernels are examples of non-device management operations).
    * This error can also be returned if using runtime/driver
    * interoperability and there is an existing ::CUcontext active on the
    * host thread.
	*/
    [Serializable]
	internal class CudaErrorSetOnActiveProcessException : Exception
    {
        public CudaErrorSetOnActiveProcessException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates that the surface passed to the API call is not a valid
    * surface.
	*/
    [Serializable]
	internal class CudaErrorInvalidSurfaceException : Exception
    {
        public CudaErrorInvalidSurfaceException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates that no CUDA-capable devices were detected by the installed
    * CUDA driver.
	*/
    [Serializable]
	internal class CudaErrorNoDeviceException : Exception
    {
        public CudaErrorNoDeviceException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates that an uncorrectable ECC error was detected during
    * execution.
	*/
    [Serializable]
	internal class CudaErrorECCUncorrectableException : Exception
    {
        public CudaErrorECCUncorrectableException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates that a link to a shared object failed to resolve.
	*/
    [Serializable]
	internal class CudaErrorSharedObjectSymbolNotFoundException : Exception
    {
        public CudaErrorSharedObjectSymbolNotFoundException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates that initialization of a shared object failed.
	*/
    [Serializable]
	internal class CudaErrorSharedObjectInitFailedException : Exception
    {
        public CudaErrorSharedObjectInitFailedException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates that the ::cudaLimit passed to the API call is not
    * supported by the active device.
	*/
    [Serializable]
	internal class CudaErrorUnsupportedLimitException : Exception
    {
        public CudaErrorUnsupportedLimitException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates that multiple global or constant variables (across separate
    * CUDA source files in the application) share the same string name.
	*/
    [Serializable]
	internal class CudaErrorDuplicateVariableNameException : Exception
    {
        public CudaErrorDuplicateVariableNameException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates that multiple textures (across separate CUDA source
    * files in the application) share the same string name.
	*/
    [Serializable]
	internal class CudaErrorDuplicateTextureNameException : Exception
    {
        public CudaErrorDuplicateTextureNameException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates that multiple surfaces (across separate CUDA source
    * files in the application) share the same string name.
	*/
    [Serializable]
	internal class CudaErrorDuplicateSurfaceNameException : Exception
    {
        public CudaErrorDuplicateSurfaceNameException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates that all CUDA devices are busy or unavailable at the current
    * time. Devices are often busy/unavailable due to use of
    * ::cudaComputeModeExclusive, ::cudaComputeModeProhibited or when long
    * running CUDA kernels have filled up the GPU and are blocking new work
    * from starting. They can also be unavailable due to memory constraints
    * on a device that already has active CUDA work being performed.
	*/
    [Serializable]
	internal class CudaErrorDevicesUnavailableException : Exception
    {
        public CudaErrorDevicesUnavailableException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates that the device kernel image is invalid.
	*/
    [Serializable]
	internal class CudaErrorInvalidKernelImageException : Exception
    {
        public CudaErrorInvalidKernelImageException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates that there is no kernel image available that is suitable
    * for the device. This can occur when a user specifies code generation
    * options for a particular CUDA source file that do not include the
    * corresponding device configuration.
	*/
    [Serializable]
	internal class CudaErrorNoKernelImageForDeviceException : Exception
    {
        public CudaErrorNoKernelImageForDeviceException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates that the current context is not compatible with this
    * the CUDA Runtime. This can only occur if you are using CUDA
    * Runtime/Driver interoperability and have created an existing Driver
    * context using the driver API. The Driver context may be incompatible
    * either because the Driver context was created using an older version
    * of the API, because the Runtime API call expects a primary driver
    * context and the Driver context is not primary, or because the Driver
    * context has been destroyed. Please see \ref CUDART_DRIVER Interactions
    * with the CUDA Driver API for more information.
	*/
    [Serializable]
	internal class CudaErrorIncompatibleDriverContextException : Exception
    {
        public CudaErrorIncompatibleDriverContextException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This error indicates that a call to ::cudaDeviceEnablePeerAccess() is
    * trying to re-enable peer addressing on from a context which has already
    * had peer addressing enabled.
	*/
    [Serializable]
	internal class CudaErrorPeerAccessAlreadyEnabledException : Exception
    {
        public CudaErrorPeerAccessAlreadyEnabledException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This error indicates that ::cudaDeviceDisablePeerAccess() is trying to
    * disable peer addressing which has not been enabled yet via
    * ::cudaDeviceEnablePeerAccess().
	*/
    [Serializable]
	internal class CudaErrorPeerAccessNotEnabledException : Exception
    {
        public CudaErrorPeerAccessNotEnabledException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates that a call tried to access an exclusive-thread device that
    * is already in use by a different thread.
	*/
    [Serializable]
	internal class CudaErrorDeviceAlreadyInUseException : Exception
    {
        public CudaErrorDeviceAlreadyInUseException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates profiler is not initialized for this run. This can
    * happen when the application is running with external profiling tools
    * like visual profiler.
	*/
    [Serializable]
	internal class CudaErrorProfilerDisabledException : Exception
    {
        public CudaErrorProfilerDisabledException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * \deprecated
    * This error return is deprecated as of CUDA 5.0. It is no longer an error
    * to attempt to enable/disable the profiling via ::cudaProfilerStart or
    * ::cudaProfilerStop without initialization.
	*/
    [Serializable]
	internal class CudaErrorProfilerNotInitializedException : Exception
    {
        public CudaErrorProfilerNotInitializedException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * \deprecated
    * This error return is deprecated as of CUDA 5.0. It is no longer an error
    * to call cudaProfilerStart() when profiling is already enabled.
	*/
    [Serializable]
	internal class CudaErrorProfilerAlreadyStartedException : Exception
    {
        public CudaErrorProfilerAlreadyStartedException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * \deprecated
    * This error return is deprecated as of CUDA 5.0. It is no longer an error
    * to call cudaProfilerStop() when profiling is already disabled.
	*/
    [Serializable]
	internal class CudaErrorProfilerAlreadyStoppedException : Exception
    {
        public CudaErrorProfilerAlreadyStoppedException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * An assert triggered in device code during kernel execution. The device
    * cannot be used again until ::cudaThreadExit() is called. All existing
    * allocations are invalid and must be reconstructed if the program is to
    * continue using CUDA.
	*/
    [Serializable]
	internal class CudaErrorAssertException : Exception
    {
        public CudaErrorAssertException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This error indicates that the hardware resources required to enable
    * peer access have been exhausted for one or more of the devices
    * passed to ::cudaEnablePeerAccess().
	*/
    [Serializable]
	internal class CudaErrorTooManyPeersException : Exception
    {
        public CudaErrorTooManyPeersException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This error indicates that the memory range passed to ::cudaHostRegister()
    * has already been registered.
	*/
    [Serializable]
	internal class CudaErrorHostMemoryAlreadyRegisteredException : Exception
    {
        public CudaErrorHostMemoryAlreadyRegisteredException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This error indicates that the pointer passed to ::cudaHostUnregister()
    * does not correspond to any currently registered memory region.
	*/
    [Serializable]
	internal class CudaErrorHostMemoryNotRegisteredException : Exception
    {
        public CudaErrorHostMemoryNotRegisteredException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This error indicates that an OS call failed.
	*/
    [Serializable]
	internal class CudaErrorOperatingSystemException : Exception
    {
        public CudaErrorOperatingSystemException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This error indicates that P2P access is not supported across the given
    * devices.
	*/
    [Serializable]
	internal class CudaErrorPeerAccessUnsupportedException : Exception
    {
        public CudaErrorPeerAccessUnsupportedException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This error indicates that a device runtime grid launch did not occur
    * because the depth of the child grid would exceed the maximum supported
    * number of nested grid launches.
	*/
    [Serializable]
	internal class CudaErrorLaunchMaxDepthExceededException : Exception
    {
        public CudaErrorLaunchMaxDepthExceededException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This error indicates that a grid launch did not occur because the kernel
    * uses file-scoped textures which are unsupported by the device runtime.
    * Kernels launched via the device runtime only support textures created with
    * the Texture Object API's.
	*/
    [Serializable]
	internal class CudaErrorLaunchFileScopedTexException : Exception
    {
        public CudaErrorLaunchFileScopedTexException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This error indicates that a grid launch did not occur because the kernel
    * uses file-scoped surfaces which are unsupported by the device runtime.
    * Kernels launched via the device runtime only support surfaces created with
    * the Surface Object API's.
	*/
    [Serializable]
	internal class CudaErrorLaunchFileScopedSurfException : Exception
    {
        public CudaErrorLaunchFileScopedSurfException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This error indicates that a call to ::cudaDeviceSynchronize made from
    * the device runtime failed because the call was made at grid depth greater
    * than than either the default (2 levels of grids) or user specified device
    * limit ::cudaLimitDevRuntimeSyncDepth. To be able to synchronize on
    * launched grids at a greater depth successfully, the maximum nested
    * depth at which ::cudaDeviceSynchronize will be called must be specified
    * with the ::cudaLimitDevRuntimeSyncDepth limit to the ::cudaDeviceSetLimit
    * api before the host-side launch of a kernel using the device runtime.
    * Keep in mind that additional levels of sync depth require the runtime
    * to reserve large amounts of device memory that cannot be used for
    * user allocations.
	*/
    [Serializable]
	internal class CudaErrorSyncDepthExceededException : Exception
    {
        public CudaErrorSyncDepthExceededException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This error indicates that a device runtime grid launch failed because
    * the launch would exceed the limit ::cudaLimitDevRuntimePendingLaunchCount.
    * For this launch to proceed successfully, ::cudaDeviceSetLimit must be
    * called to set the ::cudaLimitDevRuntimePendingLaunchCount to be higher
    * than the upper bound of outstanding launches that can be issued to the
    * device runtime. Keep in mind that raising the limit of pending device
    * runtime launches will require the runtime to reserve device memory that
    * cannot be used for user allocations.
	*/
    [Serializable]
	internal class CudaErrorLaunchPendingCountExceededException : Exception
    {
        public CudaErrorLaunchPendingCountExceededException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This error indicates the attempted operation is not permitted.
	*/
    [Serializable]
	internal class CudaErrorNotPermittedException : Exception
    {
        public CudaErrorNotPermittedException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This error indicates the attempted operation is not supported
    * on the current system or device.
	*/
    [Serializable]
	internal class CudaErrorNotSupportedException : Exception
    {
        public CudaErrorNotSupportedException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * Device encountered an error in the call stack during kernel execution,
    * possibly due to stack corruption or exceeding the stack size limit.
    * This leaves the process in an inconsistent state and any further CUDA work
    * will return the same error. To continue using CUDA, the process must be terminated
    * and relaunched.
	*/
    [Serializable]
	internal class CudaErrorHardwareStackErrorException : Exception
    {
        public CudaErrorHardwareStackErrorException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * The device encountered an illegal instruction during kernel execution
    * This leaves the process in an inconsistent state and any further CUDA work
    * will return the same error. To continue using CUDA, the process must be terminated
    * and relaunched.
	*/
    [Serializable]
	internal class CudaErrorIllegalInstructionException : Exception
    {
        public CudaErrorIllegalInstructionException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * The device encountered a load or store instruction
    * on a memory address which is not aligned.
    * This leaves the process in an inconsistent state and any further CUDA work
    * will return the same error. To continue using CUDA, the process must be terminated
    * and relaunched.
	*/
    [Serializable]
	internal class CudaErrorMisalignedAddressException : Exception
    {
        public CudaErrorMisalignedAddressException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * While executing a kernel, the device encountered an instruction
    * which can only operate on memory locations in certain address spaces
    * (global, shared, or local), but was supplied a memory address not
    * belonging to an allowed address space.
    * This leaves the process in an inconsistent state and any further CUDA work
    * will return the same error. To continue using CUDA, the process must be terminated
    * and relaunched.
	*/
    [Serializable]
	internal class CudaErrorInvalidAddressSpaceException : Exception
    {
        public CudaErrorInvalidAddressSpaceException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * The device encountered an invalid program counter.
    * This leaves the process in an inconsistent state and any further CUDA work
    * will return the same error. To continue using CUDA, the process must be terminated
    * and relaunched.
	*/
    [Serializable]
	internal class CudaErrorInvalidPcException : Exception
    {
        public CudaErrorInvalidPcException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * The device encountered a load or store instruction on an invalid memory address.
    * This leaves the process in an inconsistent state and any further CUDA work
    * will return the same error. To continue using CUDA, the process must be terminated
    * and relaunched.
	*/
    [Serializable]
	internal class CudaErrorIllegalAddressException : Exception
    {
        public CudaErrorIllegalAddressException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * A PTX compilation failed. The runtime may fall back to compiling PTX if
    * an application does not contain a suitable binary for the current device.
	*/
    [Serializable]
	internal class CudaErrorInvalidPtxException : Exception
    {
        public CudaErrorInvalidPtxException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates an error with the OpenGL or DirectX context.
	*/
    [Serializable]
	internal class CudaErrorInvalidGraphicsContextException : Exception
    {
        public CudaErrorInvalidGraphicsContextException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates that an uncorrectable NVLink error was detected during the
    * execution.
	*/
    [Serializable]
	internal class CudaErrorNvlinkUncorrectableException : Exception
    {
        public CudaErrorNvlinkUncorrectableException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates that the PTX JIT compiler library was not found. The JIT Compiler
    * library is used for PTX compilation. The runtime may fall back to compiling PTX
    * if an application does not contain a suitable binary for the current device.
	*/
    [Serializable]
	internal class CudaErrorJitCompilerNotFoundException : Exception
    {
        public CudaErrorJitCompilerNotFoundException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This error indicates that the number of blocks launched per grid for a kernel that was
    * launched via either ::cudaLaunchCooperativeKernel or ::cudaLaunchCooperativeKernelMultiDevice
    * exceeds the maximum number of blocks as allowed by ::cudaOccupancyMaxActiveBlocksPerMultiprocessor
    * or ::cudaOccupancyMaxActiveBlocksPerMultiprocessorWithFlags times the number of multiprocessors
    * as specified by the device attribute ::cudaDevAttrMultiProcessorCount.
	*/
    [Serializable]
	internal class CudaErrorCooperativeLaunchTooLargeException : Exception
    {
        public CudaErrorCooperativeLaunchTooLargeException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * This indicates an internal startup failure in the CUDA runtime.
	*/
    [Serializable]
	internal class CudaErrorStartupFailureException : Exception
    {
        public CudaErrorStartupFailureException(string kernelName) : base(kernelName)
        {
        }
    };

    /**
    * Any unhandled CUDA driver error is added to this value and returned via
    * the runtime. Production releases of CUDA should not return such errors.
    * \deprecated
    * This error return is deprecated as of CUDA 4.1.
	*/
    [Serializable]
	internal class CudaErrorApiFailureBaseException : Exception
    {
        public CudaErrorApiFailureBaseException(string kernelName) : base(kernelName)
        {
        }
    };

    #endregion

    #region CuBlas Exception mapping

    [Serializable]
	internal class CuBlasNotInitialisedException : Exception
    {
        public CuBlasNotInitialisedException(string kernelName) : base(kernelName)
        {
        }
    };

    [Serializable]
	internal class CuBlasAllocFailedException : Exception
    {
        public CuBlasAllocFailedException(string kernelName) : base(kernelName)
        {
        }
    };

    [Serializable]
	internal class CuBlasInvalidValueException : Exception
    {
        public CuBlasInvalidValueException(string kernelName) : base(kernelName)
        {
        }
    };

    [Serializable]
	internal class CuBlasArchMismatchException : Exception
    {
        public CuBlasArchMismatchException(string kernelName) : base(kernelName)
        {
        }
    };

    [Serializable]
	internal class CuBlasMappingErrorException : Exception
    {
        public CuBlasMappingErrorException(string kernelName) : base(kernelName)
        {
        }
    };

    [Serializable]
	internal class CuBlasExecutionFailedException : Exception
    {
        public CuBlasExecutionFailedException(string kernelName) : base(kernelName)
        {
        }
    };

    [Serializable]
	internal class CuBlasInternalErrorException : Exception
    {
        public CuBlasInternalErrorException(string kernelName) : base(kernelName)
        {
        }
    };

    [Serializable]
	internal class CuBlasNotSupportedException : Exception
    {
        public CuBlasNotSupportedException(string kernelName) : base(kernelName)
        {
        }
    };

    [Serializable]
	internal class CuBlasLicenseErrorException : Exception
    {
        public CuBlasLicenseErrorException(string kernelName) : base(kernelName)
        {
        }
    };

    #endregion

    #region CuSparse Exception mapping

    [Serializable]
	internal class CuSparseNotInitialisedException : Exception
    {
        public CuSparseNotInitialisedException(string kernelName) : base(kernelName)
        {
        }
    };

    [Serializable]
	internal class CuSparseAllocFailedException : Exception
    {
        public CuSparseAllocFailedException(string kernelName) : base(kernelName)
        {
        }
    };

    [Serializable]
	internal class CuSparseInvalidValueException : Exception
    {
        public CuSparseInvalidValueException(string kernelName) : base(kernelName)
        {
        }
    };

    [Serializable]
	internal class CuSparseArchMismatchException : Exception
    {
        public CuSparseArchMismatchException(string kernelName) : base(kernelName)
        {
        }
    };

    [Serializable]
	internal class CuSparseMappingErrorException : Exception
    {
        public CuSparseMappingErrorException(string kernelName) : base(kernelName)
        {
        }
    };

    [Serializable]
	internal class CuSparseExecutionFailedException : Exception
    {
        public CuSparseExecutionFailedException(string kernelName) : base(kernelName)
        {
        }
    };

    [Serializable]
	internal class CuSparseInternalErrorException : Exception
    {
        public CuSparseInternalErrorException(string kernelName) : base(kernelName)
        {
        }
    };

    [Serializable]
	internal class CuSparseMatrixTypeNotSupportedException : Exception
    {
        public CuSparseMatrixTypeNotSupportedException(string kernelName) : base(kernelName)
        {
        }
    };

    [Serializable]
	internal class CuSparseZeroPivotException : Exception
    {
        public CuSparseZeroPivotException(string kernelName) : base(kernelName)
        {
        }
    };

    #endregion


    // CudaLightInternalExceptions
    internal struct CudaGenericKernelExceptionFactory
    {
        public static void ThrowException(string kernelName, int errorCode)
        {
            switch (errorCode)
            {
                case -4:
                    throw new InternalErrorException(kernelName);
                case -3:
                    throw new ExpectedEvenSizeException(kernelName);
                case -2:
                    throw new NotSupportedException(kernelName);
                case -1:
                default:
                    throw new NotImplementedException(kernelName);
            }
        }
    };

    internal struct CudaKernelExceptionFactory
    {
        public static void ThrowException(string kernelName, int errorCode)
        {
            switch (errorCode)
            {
                // CudaLightInternalExceptions
                case -3:
                case -2:
                case -1:
                    CudaGenericKernelExceptionFactory.ThrowException(kernelName, errorCode);
                    break;
                case 1:
                    throw new CudaErrorMissingConfigurationException(kernelName);
                case 2:
                    throw new CudaErrorMemoryAllocationException(kernelName);
                case 3:
                    throw new CudaErrorInitializationErrorException(kernelName);
                case 4:
                    throw new CudaErrorLaunchFailureException(kernelName);
                case 5:
                    throw new CudaErrorPriorLaunchFailureException(kernelName);
                case 6:
                    throw new CudaErrorLaunchTimeoutException(kernelName);
                case 7:
                    throw new CudaErrorLaunchOutOfResourcesException(kernelName);
                case 8:
                    throw new CudaErrorInvalidDeviceFunctionException(kernelName);
                case 9:
                    throw new CudaErrorInvalidConfigurationException(kernelName);
                case 10:
                    throw new CudaErrorInvalidDeviceException(kernelName);
                case 11:
                    throw new CudaErrorInvalidValueException(kernelName);
                case 12:
                    throw new CudaErrorInvalidPitchValueException(kernelName);
                case 13:
                    throw new CudaErrorInvalidSymbolException(kernelName);
                case 14:
                    throw new CudaErrorMapBufferObjectFailedException(kernelName);
                case 15:
                    throw new CudaErrorUnmapBufferObjectFailedException(kernelName);
                case 16:
                    throw new CudaErrorInvalidHostPointerException(kernelName);
                case 17:
                    throw new CudaErrorInvalidDevicePointerException(kernelName);
                case 18:
                    throw new CudaErrorInvalidTextureException(kernelName);
                case 19:
                    throw new CudaErrorInvalidTextureBindingException(kernelName);
                case 20:
                    throw new CudaErrorInvalidChannelDescriptorException(kernelName);
                case 21:
                    throw new CudaErrorInvalidMemcpyDirectionException(kernelName);
                case 22:
                    throw new CudaErrorAddressOfConstantException(kernelName);
                case 23:
                    throw new CudaErrorTextureFetchFailedException(kernelName);
                case 24:
                    throw new CudaErrorTextureNotBoundException(kernelName);
                case 25:
                    throw new CudaErrorSynchronizationErrorException(kernelName);
                case 26:
                    throw new CudaErrorInvalidFilterSettingException(kernelName);
                case 27:
                    throw new CudaErrorInvalidNormSettingException(kernelName);
                case 28:
                    throw new CudaErrorMixedDeviceExecutionException(kernelName);
                case 29:
                    throw new CudaErrorCudartUnloadingException(kernelName);
                case 30:
                    throw new CudaErrorUnknownException(kernelName);
                case 31:
                    throw new CudaErrorNotYetImplementedException(kernelName);
                case 32:
                    throw new CudaErrorMemoryValueTooLargeException(kernelName);
                case 33:
                    throw new CudaErrorInvalidResourceHandleException(kernelName);
                case 34:
                    throw new CudaErrorNotReadyException(kernelName);
                case 35:
                    throw new CudaErrorInsufficientDriverException(kernelName);
                case 36:
                    throw new CudaErrorSetOnActiveProcessException(kernelName);
                case 37:
                    throw new CudaErrorInvalidSurfaceException(kernelName);
                case 38:
                    throw new CudaErrorNoDeviceException(kernelName);
                case 39:
                    throw new CudaErrorECCUncorrectableException(kernelName);
                case 40:
                    throw new CudaErrorSharedObjectSymbolNotFoundException(kernelName);
                case 41:
                    throw new CudaErrorSharedObjectInitFailedException(kernelName);
                case 42:
                    throw new CudaErrorUnsupportedLimitException(kernelName);
                case 43:
                    throw new CudaErrorDuplicateVariableNameException(kernelName);
                case 44:
                    throw new CudaErrorDuplicateTextureNameException(kernelName);
                case 45:
                    throw new CudaErrorDuplicateSurfaceNameException(kernelName);
                case 46:
                    throw new CudaErrorDevicesUnavailableException(kernelName);
                case 47:
                    throw new CudaErrorInvalidKernelImageException(kernelName);
                case 48:
                    throw new CudaErrorNoKernelImageForDeviceException(kernelName);
                case 49:
                    throw new CudaErrorIncompatibleDriverContextException(kernelName);
                case 50:
                    throw new CudaErrorPeerAccessAlreadyEnabledException(kernelName);
                case 51:
                    throw new CudaErrorPeerAccessNotEnabledException(kernelName);
                case 54:
                    throw new CudaErrorDeviceAlreadyInUseException(kernelName);
                case 55:
                    throw new CudaErrorProfilerDisabledException(kernelName);
                case 56:
                    throw new CudaErrorProfilerNotInitializedException(kernelName);
                case 57:
                    throw new CudaErrorProfilerAlreadyStartedException(kernelName);
                case 58:
                    throw new CudaErrorProfilerAlreadyStoppedException(kernelName);
                case 59:
                    throw new CudaErrorAssertException(kernelName);
                case 60:
                    throw new CudaErrorTooManyPeersException(kernelName);
                case 61:
                    throw new CudaErrorHostMemoryAlreadyRegisteredException(kernelName);
                case 62:
                    throw new CudaErrorHostMemoryNotRegisteredException(kernelName);
                case 63:
                    throw new CudaErrorOperatingSystemException(kernelName);
                case 64:
                    throw new CudaErrorPeerAccessUnsupportedException(kernelName);
                case 65:
                    throw new CudaErrorLaunchMaxDepthExceededException(kernelName);
                case 66:
                    throw new CudaErrorLaunchFileScopedTexException(kernelName);
                case 67:
                    throw new CudaErrorLaunchFileScopedSurfException(kernelName);
                case 68:
                    throw new CudaErrorSyncDepthExceededException(kernelName);
                case 69:
                    throw new CudaErrorLaunchPendingCountExceededException(kernelName);
                case 70:
                    throw new CudaErrorNotPermittedException(kernelName);
                case 71:
                    throw new CudaErrorNotSupportedException(kernelName);
                case 72:
                    throw new CudaErrorHardwareStackErrorException(kernelName);
                case 73:
                    throw new CudaErrorIllegalInstructionException(kernelName);
                case 74:
                    throw new CudaErrorMisalignedAddressException(kernelName);
                case 75:
                    throw new CudaErrorInvalidAddressSpaceException(kernelName);
                case 76:
                    throw new CudaErrorInvalidPcException(kernelName);
                case 77:
                    throw new CudaErrorIllegalAddressException(kernelName);
                case 78:
                    throw new CudaErrorInvalidPtxException(kernelName);
                case 79:
                    throw new CudaErrorInvalidGraphicsContextException(kernelName);
                case 80:
                    throw new CudaErrorNvlinkUncorrectableException(kernelName);
                case 81:
                    throw new CudaErrorJitCompilerNotFoundException(kernelName);
                case 82:
                    throw new CudaErrorCooperativeLaunchTooLargeException(kernelName);
                case 0x7f:
                    throw new CudaErrorStartupFailureException(kernelName);
                case 10000:
                    throw new CudaErrorApiFailureBaseException(kernelName);
                default:
                    throw new CudaGenericKernelException(kernelName, errorCode);
            }
        }
    };

    internal struct CuBlasKernelExceptionFactory
    {
        public static void ThrowException(string kernelName, int errorCode)
        {
            switch (errorCode)
            {
                // CudaLightInternalExceptions
                case -3:
                case -2:
                case -1:
                    CudaGenericKernelExceptionFactory.ThrowException(kernelName, errorCode);
                    break;
                case 1:
                    throw new CuBlasNotInitialisedException(kernelName);
                case 3:
                    throw new CuBlasAllocFailedException(kernelName);
                case 7:
                    throw new CuBlasInvalidValueException(kernelName);
                case 8:
                    throw new CuBlasArchMismatchException(kernelName);
                case 11:
                    throw new CuBlasMappingErrorException(kernelName);
                case 13:
                    throw new CuBlasExecutionFailedException(kernelName);
                case 14:
                    throw new CuBlasInternalErrorException(kernelName);
                case 15:
                    throw new CuBlasNotSupportedException(kernelName);
                case 16:
                    throw new CuBlasLicenseErrorException(kernelName);
                default:
                    throw new CudaGenericKernelException(kernelName, errorCode);
            }
        }
    };

    struct CuSparseKernelExceptionFactory
    {
        static void ThrowException(string kernelName, int errorCode)
        {
            switch (errorCode)
            {
                // CudaLightInternalExceptions
                case -3:
                case -2:
                case -1:
                    CudaGenericKernelExceptionFactory.ThrowException(kernelName, errorCode);
                    break;
                case 1:
                    throw new CuSparseNotInitialisedException(kernelName);
                case 2:
                    throw new CuSparseAllocFailedException(kernelName);
                case 3:
                    throw new CuSparseInvalidValueException(kernelName);
                case 4:
                    throw new CuSparseArchMismatchException(kernelName);
                case 5:
                    throw new CuSparseMappingErrorException(kernelName);
                case 6:
                    throw new CuSparseExecutionFailedException(kernelName);
                case 7:
                    throw new CuSparseInternalErrorException(kernelName);
                case 8:
                    throw new CuSparseMatrixTypeNotSupportedException(kernelName);
                case 9:
                    throw new CuSparseZeroPivotException(kernelName);
                default:
                    throw new CudaGenericKernelException(kernelName, errorCode);
            }
        }
    };
}
