import { fetchCurrentUser as fetchAuthCurrentUser, updateProfile as updateAuthProfile } from '../auth/api';
import type { ProfileUser, UpdateProfilePayload } from './types';

export async function fetchCurrentUser(): Promise<ProfileUser> {
  return fetchAuthCurrentUser();
}

export async function updateProfile(payload: UpdateProfilePayload): Promise<ProfileUser> {
  return updateAuthProfile(payload);
}
