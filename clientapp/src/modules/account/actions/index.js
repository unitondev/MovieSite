import { createAction } from 'redux-actions'

export const loginRequest = createAction('LOGIN_REQUEST')
export const loginSuccess = createAction('LOGIN_SUCCESS')
export const loginFailure = createAction('LOGIN_FAILURE')

export const logoutRequest = createAction('LOGOUT_REQUEST')
export const logoutSuccess = createAction('LOGOUT_SUCCESS')
export const logoutFailure = createAction('LOGOUT_FAILURE')

export const refreshTokensRequest = createAction('REFRESH_TOKENS_REQUEST')
export const refreshTokensSuccess = createAction('REFRESH_TOKENS_SUCCESS')
export const refreshTokensFailure = createAction('REFRESH_TOKENS_FAILURE')

export const updateUserRequest = createAction('UPDATE_USER_REQUEST')
export const updateUserSuccess = createAction('UPDATE_USER_SUCCESS')
export const updateUserFailure = createAction('UPDATE_USER_FAILURE')

export const deleteUserRequest = createAction('DELETE_USER_REQUEST')
export const deleteUserSuccess = createAction('DELETE_USER_SUCCESS')
export const deleteUserFailure = createAction('DELETE_USER_FAILURE')

export const registerRequest = createAction('REGISTER_REQUEST')
export const registerSuccess = createAction('REGISTER_SUCCESS')
export const registerFailure = createAction('REGISTER_FAILURE')

export const startLoadingUser = createAction('START_LOADING_USER')
export const stopLoadingUser = createAction('STOP_LOADING_USER')