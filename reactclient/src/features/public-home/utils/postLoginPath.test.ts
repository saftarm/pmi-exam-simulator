import assert from 'node:assert/strict';
import { describe, it } from 'node:test';
import {
  resolvePostLoginPath,
  resolveAuthenticatedHomePath,
  isPathAllowedForRole,
} from './postLoginPath.ts';

const learner = { role: 'Learner' };
const admin = { role: 'Admin' };

describe('resolvePostLoginPath', () => {
  it('sends learner away from /admin to /exams', () => {
    assert.equal(resolvePostLoginPath(learner, '/admin'), '/exams');
    assert.equal(resolvePostLoginPath(learner, '/admin/exams'), '/exams');
  });

  it('preserves allowed learner destinations', () => {
    assert.equal(resolvePostLoginPath(learner, '/exams'), '/exams');
    assert.equal(resolvePostLoginPath(learner, '/profile'), '/profile');
  });

  it('defaults learner to /exams when from is missing', () => {
    assert.equal(resolvePostLoginPath(learner, undefined), '/exams');
    assert.equal(resolvePostLoginPath(learner, '/login'), '/exams');
  });

  it('preserves admin admin-area destinations', () => {
    assert.equal(resolvePostLoginPath(admin, '/admin/users'), '/admin/users');
  });

  it('defaults admin to /admin when from is learner route or missing', () => {
    assert.equal(resolvePostLoginPath(admin, '/exams'), '/admin');
    assert.equal(resolvePostLoginPath(admin, undefined), '/admin');
  });

  it('preserves shared routes for admin (e.g. profile)', () => {
    assert.equal(resolvePostLoginPath(admin, '/profile'), '/profile');
    assert.equal(resolvePostLoginPath(admin, '/about'), '/about');
  });
});

describe('resolveAuthenticatedHomePath', () => {
  it('returns role-appropriate home', () => {
    assert.equal(resolveAuthenticatedHomePath(learner), '/exams');
    assert.equal(resolveAuthenticatedHomePath(admin), '/admin');
  });
});

describe('isPathAllowedForRole', () => {
  it('blocks admin paths for non-admins', () => {
    assert.equal(isPathAllowedForRole('/admin', 'Learner'), false);
    assert.equal(isPathAllowedForRole('/admin/settings', 'Learner'), false);
  });

  it('allows admin paths for admins', () => {
    assert.equal(isPathAllowedForRole('/admin', 'Admin'), true);
  });
});
