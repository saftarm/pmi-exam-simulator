import { createContext, useContext, useState, useCallback } from 'react';
import * as authService from '../services/authService';

const AuthContext = createContext(null);

export function AuthProvider({ children }) {
    const [user, setUser] = useState(() => authService.getStoredUser());

    const login = useCallback(async (userName, password) => {
        const loggedInUser = await authService.login(userName, password);
        setUser(loggedInUser);
        return loggedInUser;
    }, []);

    const register = useCallback(async (payload) => {
        await authService.register(payload);
    }, []);

    const logout = useCallback(() => {
        authService.logout();
        setUser(null);
    }, []);

    return (
        <AuthContext.Provider
            value={{
                user,
                isAuthenticated: !!user,
                login,
                register,
                logout,
            }}
        >
            {children}
        </AuthContext.Provider>
    );
}

export function useAuth() {
    const ctx = useContext(AuthContext);
    if (!ctx) throw new Error('useAuth must be used within AuthProvider');
    return ctx;
}
