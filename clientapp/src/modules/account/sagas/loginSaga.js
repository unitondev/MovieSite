import { all, put, take, takeLatest } from 'redux-saga/effects'

import * as accountActions from '../actions'
import * as notificationActions from '../../shared/snackBarNotification/actions'
import routes from '@movie/routes'

function * onLogin(action) {
  yield put(accountActions.getTokenRequest(action.payload))
  const result = yield take([accountActions.getTokenSuccess, accountActions.getTokenFailure])
  if (result.type === accountActions.getTokenFailure().type) {
    // push from connected-react-router
    // yield put(push(routes.login))
    return
  }

  yield put(accountActions.getMeRequest())
  // TODO add redirect here if exists
}

function * onRefreshToken(action) {
  yield put(accountActions.refreshTokensRequest())
  const result = yield take([accountActions.refreshTokensSuccess, accountActions.refreshTokensFailure])
  if (result.type === accountActions.refreshTokensSuccess().type) {
    yield put(accountActions.getMeRequest())
  }
}

function * onRegister(action) {
  yield put(accountActions.registerRequest(action.payload))
  const result = yield take([accountActions.registerSuccess, accountActions.registerFailure])
  if (result.type === accountActions.registerFailure().type) {
    const message = 'Register failure'
    yield put(notificationActions.enqueueSnackbarError({ message }))
    return
  }

  yield put(accountActions.getMeRequest())
}

function * loginSaga() {
  yield all([
    takeLatest(accountActions.onLogin, onLogin),
    takeLatest(accountActions.onRefreshToken, onRefreshToken),
    takeLatest(accountActions.onRegister, onRegister),
  ])
}

export default loginSaga