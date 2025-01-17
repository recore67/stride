// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org/ & https://stride3d.net)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using Stride.Games;

namespace Stride.Input
{
    /// <summary>
    /// Given a <see cref="GameContext"/> creates the corresponding <see cref="IInputSource"/> for the platform specific window.
    /// </summary>
    public static class InputSourceFactory
    {
        /// <summary>
        /// Creates a new input source for the window provided by the <paramref name="context"/>.
        /// </summary>
        /// <param name="context">The context containing the platform specific window.</param>
        /// <returns>An input source for the window.</returns>
        public static IInputSource NewWindowInputSource(GameContext context)
        {
            switch (context.ContextType)
            {
#if STRIDE_UI_SDL
                case AppContextType.DesktopSDL:
                    var sdlContext = (GameContextSDL)context;
                    return new InputSourceSDL(sdlContext.Control);
#endif
#if (STRIDE_UI_WINFORMS || STRIDE_UI_WPF)
                case AppContextType.Desktop:
                    var winformsContext = (GameContextWinforms)context;
                    return new InputSourceWinforms(winformsContext.Control);
#endif
                default:
                    throw new InvalidOperationException("GameContext type is not supported by the InputManager");
            }
        }
    }
}
