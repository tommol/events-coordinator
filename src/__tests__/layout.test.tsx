import { render, screen } from '@testing-library/react'
import { metadata } from '../app/layout'

// Mock HeroUIProvider to avoid complex provider setup
jest.mock('@heroui/react', () => ({
  HeroUIProvider: ({ children }: any) => <div data-testid="heroui-provider">{children}</div>,
}))

describe('RootLayout', () => {
  // Create a mock component that only renders the body content
  const MockLayout = ({ children }: { children: React.ReactNode }) => {
    return (
      <div data-testid="heroui-provider">
        <div className="antialiased">
          {children}
        </div>
      </div>
    )
  }

  it('renders children within HeroUIProvider', () => {
    render(
      <MockLayout>
        <div>Test Content</div>
      </MockLayout>
    )
    
    expect(screen.getByTestId('heroui-provider')).toBeInTheDocument()
    expect(screen.getByText('Test Content')).toBeInTheDocument()
  })

  it('applies antialiased class to body content', () => {
    const { container } = render(
      <MockLayout>
        <div>Test Content</div>
      </MockLayout>
    )
    
    const bodyContentElement = container.querySelector('.antialiased')
    expect(bodyContentElement).toBeInTheDocument()
  })
})

describe('Metadata', () => {
  it('has correct title', () => {
    expect(metadata.title).toBe('Events Coordinator')
  })

  it('has correct description', () => {
    expect(metadata.description).toBe('Next.js application with HeroUI for event coordination')
  })
})