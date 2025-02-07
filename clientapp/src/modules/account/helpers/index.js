import React, { useEffect, useState } from 'react'
import { Avatar } from '@mui/material'
import { ManageAccounts, Password, Subscriptions } from '@mui/icons-material'

export const toBase64 = (file) =>
  new Promise((resolve, reject) => {
    const reader = new FileReader()
    reader.readAsDataURL(file)
    reader.onload = () => resolve(reader.result)
    reader.onerror = (error) => reject(error)
  })

export const AvatarPreview = ({ file, ...props }) => {
  const [previewString, setPreviewString] = useState(null)

  useEffect(() => {
    if (!!file && typeof file === 'object') {
      let reader = new FileReader()
      reader.onloadend = () => {
        setPreviewString(reader.result)
      }
      reader.readAsDataURL(file)
    } else if (typeof file === 'string') {
      setPreviewString(file)
    }
  }, [file])

  return <Avatar src={previewString && previewString} {...props} />
}

export const getUserSettingsTabs = () => [
  { id: 0, label: 'Account', icon: <ManageAccounts /> },
  { id: 1, label: 'Subscriptions', icon: <Subscriptions /> },
  { id: 2, label: 'Password', icon: <Password /> },
]
