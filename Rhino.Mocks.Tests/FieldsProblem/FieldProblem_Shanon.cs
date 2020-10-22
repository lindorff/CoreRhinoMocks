#region license
// Copyright (c) 2020 rubicon IT GmbH, www.rubicon.eu
// Copyright (c) 2005 - 2009 Ayende Rahien (ayende@ayende.com)
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
// 
//     * Redistributions of source code must retain the above copyright notice,
//     this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice,
//     this list of conditions and the following disclaimer in the documentation
//     and/or other materials provided with the distribution.
//     * Neither the name of Ayende Rahien nor the names of its
//     contributors may be used to endorse or promote products derived from this
//     software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
// THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#endregion

using NUnit.Framework;
using RhinoMocksCPPInterfaces;

namespace Rhino.Mocks.Tests.FieldsProblem
{
	
	public class FieldProblem_Shanon
	{
		[Test, Ignore(@"Updating the Castle and NH assmeblies causes this to fail.
		
		Message:Method 'StartLiveOnSlot' in type 'IHaveMethodWithModOptsProxye59cf24cdfbc4797af58984e3c4fdf3f' from assembly 'DynamicProxyGenAssembly2, Version=0.0.0.0, Culture=neutral, PublicKeyToken=a621a9e7e5c32e69' does not have an implementation.
Source:mscorlib
TypeName:IHaveMethodWithModOptsProxye59cf24cdfbc4797af58984e3c4fdf3f
TargetSite:System.Type _TermCreateClass(Int32, System.Reflection.Module)
HelpLink:null
StackTrace:

   at System.Reflection.Emit.TypeBuilder._TermCreateClass(Int32 handle, Module module)
   at System.Reflection.Emit.TypeBuilder.CreateTypeNoLock()
   at System.Reflection.Emit.TypeBuilder.CreateType()
   at Castle.DynamicProxy.Generators.Emitters.AbstractTypeEmitter.BuildType()
   at Castle.DynamicProxy.Generators.InterfaceProxyWithTargetGenerator.GenerateCode(Type proxyTargetType, Type[] interfaces, ProxyGenerationOptions options)
   at Castle.DynamicProxy.DefaultProxyBuilder.CreateInterfaceProxyTypeWithoutTarget(Type theInterface, Type[] interfaces, ProxyGenerationOptions options)
   at Castle.DynamicProxy.ProxyGenerator.CreateInterfaceProxyTypeWithoutTarget(Type theInterface, Type[] interfaces, ProxyGenerationOptions options)
   at Castle.DynamicProxy.ProxyGenerator.CreateInterfaceProxyWithoutTarget(Type theInterface, Type[] interfaces, ProxyGenerationOptions options, IInterceptor[] interceptors)
   at Castle.DynamicProxy.ProxyGenerator.CreateInterfaceProxyWithoutTarget(Type theInterface, Type[] interfaces, IInterceptor[] interceptors)
   at Rhino.Mocks.MockRepository.MockInterface(CreateMockState mockStateFactory, Type type, Type[] extras)
   at Rhino.Mocks.MockRepository.CreateMockObject(Type type, CreateMockState factory, Type[] extras, Object[] argumentsForConstructor)
   at Rhino.Mocks.MockRepository.StrictMock[T](Object[] argumentsForConstructor)
   at Rhino.Mocks.Tests.FieldsProblem.FieldProblem_Shanon.CanMockInterfaceWithMethodsHavingModOpt() in c:\Documents and Settings\jmeckley\My Documents\Visual Studio 2005\Projects\Rhino-Tools\trunk\rhino-mocks\Rhino.Mocks.Tests\FieldsProblem\FieldProblem_Shanon.cs:line 13")]
		public void CanMockInterfaceWithMethodsHavingModOpt()
		{
			MockRepository mocks = new MockRepository();
			IHaveMethodWithModOpts mock = mocks.StrictMock<IHaveMethodWithModOpts>();
			Assert.NotNull(mock);
		}
	}
}
