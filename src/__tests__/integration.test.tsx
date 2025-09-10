import { render, screen } from '@testing-library/react'
import { HeroUIProvider } from '@heroui/react'
import Home from '../app/page'

// Mock HeroUI components for integration testing
jest.mock('@heroui/react', () => ({
  ...jest.requireActual('@heroui/react'),
  Button: ({ children, color, size, variant, onClick, ...props }: any) => (
    <button 
      data-color={color} 
      data-size={size} 
      data-variant={variant} 
      onClick={onClick}
      {...props}
    >
      {children}
    </button>
  ),
  Card: ({ children, ...props }: any) => <div data-testid="card" {...props}>{children}</div>,
  CardBody: ({ children, ...props }: any) => <div data-testid="card-body" {...props}>{children}</div>,
  CardHeader: ({ children, ...props }: any) => <div data-testid="card-header" {...props}>{children}</div>,
  Navbar: ({ children, className, ...props }: any) => (
    <nav data-testid="navbar" className={className} {...props}>{children}</nav>
  ),
  NavbarBrand: ({ children, ...props }: any) => <div data-testid="navbar-brand" {...props}>{children}</div>,
  NavbarContent: ({ children, justify, ...props }: any) => (
    <div data-testid="navbar-content" data-justify={justify} {...props}>{children}</div>
  ),
  Avatar: ({ isBordered, size, src, ...props }: any) => (
    <img data-bordered={isBordered} data-size={size} src={src} alt="avatar" {...props} />
  ),
  HeroUIProvider: ({ children }: any) => <div>{children}</div>,
}))

const renderWithProvider = (ui: React.ReactElement) => {
  return render(<HeroUIProvider>{ui}</HeroUIProvider>)
}

describe('Integration Tests', () => {
  it('displays the complete layout structure', () => {
    renderWithProvider(<Home />)
    
    // Check navigation structure
    expect(screen.getByTestId('navbar')).toBeInTheDocument()
    expect(screen.getByTestId('navbar-brand')).toBeInTheDocument()
    expect(screen.getByTestId('navbar-content')).toBeInTheDocument()
    
    // Check main content structure
    expect(screen.getByText('Welcome to Events Coordinator')).toBeInTheDocument()
    expect(screen.getByText(/A modern Next.js 15\+ application/)).toBeInTheDocument()
    
    // Check feature cards are present
    const cards = screen.getAllByTestId('card')
    expect(cards).toHaveLength(3)
    
    // Check demo buttons are present
    const buttons = screen.getAllByRole('button')
    expect(buttons.length).toBeGreaterThan(5) // Hero buttons + demo buttons
  })

  it('has proper accessibility attributes', () => {
    renderWithProvider(<Home />)
    
    // Check main heading is properly structured
    const mainHeading = screen.getByRole('heading', { level: 1 })
    expect(mainHeading).toHaveTextContent('Welcome to Events Coordinator')
    
    // Check secondary heading
    const secondaryHeading = screen.getByRole('heading', { level: 2 })
    expect(secondaryHeading).toHaveTextContent('HeroUI Components Demo')
    
    // Check navigation
    expect(screen.getByRole('navigation')).toBeInTheDocument()
    
    // Check avatar has alt text
    expect(screen.getByAltText('avatar')).toBeInTheDocument()
  })

  it('has responsive layout classes', () => {
    const { container } = renderWithProvider(<Home />)
    
    // Check main container has responsive classes
    const mainElement = container.querySelector('main')
    expect(mainElement).toHaveClass('container', 'mx-auto', 'px-4', 'py-8')
    
    // Check feature cards grid is responsive
    const featureGrid = container.querySelector('.grid.md\\:grid-cols-3')
    expect(featureGrid).toBeInTheDocument()
  })

  it('renders all expected HeroUI component variants', () => {
    renderWithProvider(<Home />)
    
    // Check different button variants in demo section
    expect(screen.getByText('Primary')).toBeInTheDocument()
    expect(screen.getByText('Secondary')).toBeInTheDocument()  
    expect(screen.getByText('Success')).toBeInTheDocument()
    expect(screen.getByText('Warning')).toBeInTheDocument()
    expect(screen.getByText('Danger')).toBeInTheDocument()
  })

  it('has proper gradient background styling', () => {
    const { container } = renderWithProvider(<Home />)
    
    const mainContainer = container.querySelector('.min-h-screen')
    expect(mainContainer).toHaveClass(
      'bg-gradient-to-br',
      'from-blue-50',
      'to-indigo-100',
      'dark:from-gray-900',
      'dark:to-gray-800'
    )
  })
})